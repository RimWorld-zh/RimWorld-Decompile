using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		protected float growthInt = 0.05f;

		protected int ageInt = 0;

		protected int unlitTicks = 0;

		protected int madeLeaflessTick = -99999;

		public bool sown = false;

		private string cachedLabelMouseover = null;

		private static Color32[] workingColors = new Color32[4];

		public const float BaseGrowthPercent = 0.05f;

		private const float BaseDyingDamagePerTick = 0.005f;

		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		private const float GridPosRandomnessFactor = 0.3f;

		private const int TicksWithoutLightBeforeStartDying = 450000;

		private const int LeaflessMinRecoveryTicks = 60000;

		public const float MinGrowthTemperature = 0f;

		public const float MinOptimalGrowthTemperature = 10f;

		public const float MaxOptimalGrowthTemperature = 42f;

		public const float MaxGrowthTemperature = 58f;

		public const float MaxLeaflessTemperature = -2f;

		private const float MinLeaflessTemperature = -10f;

		private const float MinAnimalEatPlantsTemperature = 0f;

		public const float TopVerticesAltitudeBias = 0.1f;

		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		public Plant()
		{
		}

		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = Mathf.Clamp01(value);
				this.cachedLabelMouseover = null;
			}
		}

		public virtual int Age
		{
			get
			{
				return this.ageInt;
			}
			set
			{
				this.ageInt = value;
				this.cachedLabelMouseover = null;
			}
		}

		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		public bool HarvestableSoon
		{
			get
			{
				bool result;
				if (this.HarvestableNow)
				{
					result = true;
				}
				else if (!this.def.plant.Harvestable)
				{
					result = false;
				}
				else
				{
					float num = Mathf.Max(1f - this.Growth, 0f);
					float num2 = num * this.def.plant.growDays;
					float num3 = Mathf.Max(1f - this.def.plant.harvestMinGrowth, 0f);
					float num4 = num3 * this.def.plant.growDays;
					result = ((num2 <= 10f || num4 <= 1f) && this.GrowthRateFactor_Fertility > 0f && this.GrowthRateFactor_Temperature > 0f);
				}
				return result;
			}
		}

		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing;
			}
		}

		public Blight Blight
		{
			get
			{
				Blight result;
				if (!base.Spawned || !this.def.plant.Blightable)
				{
					result = null;
				}
				else
				{
					result = base.Position.GetFirstBlight(base.Map);
				}
				return result;
			}
		}

		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && !this.LeaflessNow && (!base.Spawned || base.Position.GetSnowDepth(base.Map) <= this.def.hideAtSnowDepth)));
			}
		}

		public virtual float CurrentDyingDamagePerTick
		{
			get
			{
				float result;
				if (!base.Spawned)
				{
					result = 0f;
				}
				else
				{
					float num = 0f;
					if (this.def.plant.LimitedLifespan && this.ageInt > this.def.plant.LifespanTicks)
					{
						num = Mathf.Max(num, 0.005f);
					}
					if (!this.def.plant.cavePlant && this.unlitTicks > 450000)
					{
						num = Mathf.Max(num, 0.005f);
					}
					if (this.DyingBecauseExposedToLight)
					{
						float lerpPct = base.Map.glowGrid.GameGlowAt(base.Position, true);
						num = Mathf.Max(num, Plant.DyingDamagePerTickBecauseExposedToLight.LerpThroughRange(lerpPct));
					}
					result = num;
				}
				return result;
			}
		}

		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
			}
		}

		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		public virtual float GrowthRate
		{
			get
			{
				float result;
				if (this.Blighted)
				{
					result = 0f;
				}
				else if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
				{
					result = 0f;
				}
				else
				{
					result = this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light;
				}
				return result;
			}
		}

		protected float GrowthPerTick
		{
			get
			{
				float result;
				if (this.LifeStage != PlantLifeStage.Growing || this.Resting)
				{
					result = 0f;
				}
				else
				{
					float num = 1f / (60000f * this.def.plant.growDays);
					result = num * this.GrowthRate;
				}
				return result;
			}
		}

		public float GrowthRateFactor_Fertility
		{
			get
			{
				return base.Map.fertilityGrid.FertilityAt(base.Position) * this.def.plant.fertilitySensitivity + (1f - this.def.plant.fertilitySensitivity);
			}
		}

		public float GrowthRateFactor_Light
		{
			get
			{
				float num = base.Map.glowGrid.GameGlowAt(base.Position, false);
				float result;
				if (this.def.plant.growMinGlow == this.def.plant.growOptimalGlow && num == this.def.plant.growOptimalGlow)
				{
					result = 1f;
				}
				else
				{
					result = GenMath.InverseLerp(this.def.plant.growMinGlow, this.def.plant.growOptimalGlow, num);
				}
				return result;
			}
		}

		public float GrowthRateFactor_Temperature
		{
			get
			{
				float num;
				float result;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
				{
					result = 1f;
				}
				else if (num < 10f)
				{
					result = Mathf.InverseLerp(0f, 10f, num);
				}
				else if (num > 42f)
				{
					result = Mathf.InverseLerp(58f, 42f, num);
				}
				else
				{
					result = 1f;
				}
				return result;
			}
		}

		protected int TicksUntilFullyGrown
		{
			get
			{
				int result;
				if (this.growthInt > 0.9999f)
				{
					result = 0;
				}
				else
				{
					float growthPerTick = this.GrowthPerTick;
					if (growthPerTick == 0f)
					{
						result = int.MaxValue;
					}
					else
					{
						result = (int)((1f - this.growthInt) / growthPerTick);
					}
				}
				return result;
			}
		}

		protected string GrowthPercentString
		{
			get
			{
				return (this.growthInt + 0.0001f).ToStringPercent();
			}
		}

		public override string LabelMouseover
		{
			get
			{
				if (this.cachedLabelMouseover == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.def.LabelCap);
					stringBuilder.Append(" (" + "PercentGrowth".Translate(new object[]
					{
						this.GrowthPercentString
					}));
					if (this.Dying)
					{
						stringBuilder.Append(", " + "DyingLower".Translate());
					}
					stringBuilder.Append(")");
					this.cachedLabelMouseover = stringBuilder.ToString();
				}
				return this.cachedLabelMouseover;
			}
		}

		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		public virtual PlantLifeStage LifeStage
		{
			get
			{
				PlantLifeStage result;
				if (this.growthInt < 0.001f)
				{
					result = PlantLifeStage.Sowing;
				}
				else if (this.growthInt > 0.999f)
				{
					result = PlantLifeStage.Mature;
				}
				else
				{
					result = PlantLifeStage.Growing;
				}
				return result;
			}
		}

		public override Graphic Graphic
		{
			get
			{
				Graphic result;
				if (this.LifeStage == PlantLifeStage.Sowing)
				{
					result = Plant.GraphicSowing;
				}
				else if (this.def.plant.leaflessGraphic != null && this.LeaflessNow && (!this.sown || !this.HarvestableNow))
				{
					result = this.def.plant.leaflessGraphic;
				}
				else if (this.def.plant.immatureGraphic != null && !this.HarvestableNow)
				{
					result = this.def.plant.immatureGraphic;
				}
				else
				{
					result = base.Graphic;
				}
				return result;
			}
		}

		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)this.HashOffset() * 0.01f % num - num + -2f;
			}
		}

		public bool IsCrop
		{
			get
			{
				bool result;
				if (!this.def.plant.Sowable)
				{
					result = false;
				}
				else if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.", false);
					result = false;
				}
				else
				{
					result = (this.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map));
				}
				return result;
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		public override void PostMapInit()
		{
			this.CheckTemperatureMakeLeafless();
		}

		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float statValue = this.GetStatValue(StatDefOf.Nutrition, true);
			if (this.def.plant.HarvestDestroys)
			{
				numTaken = 1;
			}
			else
			{
				this.growthInt -= 0.3f;
				if (this.growthInt < 0.08f)
				{
					this.growthInt = 0.08f;
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				numTaken = 0;
			}
			nutritionIngested = statValue;
		}

		public virtual void PlantCollected()
		{
			if (this.def.plant.HarvestDestroys)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				this.growthInt = this.def.plant.harvestAfterGrowth;
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold);
			}
		}

		public virtual void MakeLeafless(Plant.LeaflessCause cause)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			if (cause == Plant.LeaflessCause.Poison && this.def.plant.leaflessGraphic == null)
			{
				if (this.IsCrop)
				{
					if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
					{
						Messages.Message("MessagePlantDiedOfPoison".Translate(new object[]
						{
							this.GetCustomLabelNoCount(false)
						}).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			else if (this.def.plant.dieIfLeafless)
			{
				if (this.IsCrop)
				{
					if (cause == Plant.LeaflessCause.Cold)
					{
						if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfCold".Translate(new object[]
							{
								this.GetCustomLabelNoCount(false)
							}).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Poison)
					{
						if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfPoison".Translate(new object[]
							{
								this.GetCustomLabelNoCount(false)
							}).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			else
			{
				this.madeLeaflessTick = Find.TickManager.TicksGame;
			}
			if (flag)
			{
				map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		public override void TickLong()
		{
			this.CheckTemperatureMakeLeafless();
			if (!base.Destroyed)
			{
				if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
				{
					float num = this.growthInt;
					bool flag = this.LifeStage == PlantLifeStage.Mature;
					this.growthInt += this.GrowthPerTick * 2000f;
					if (this.growthInt > 1f)
					{
						this.growthInt = 1f;
					}
					if ((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f))
					{
						if (this.CurrentlyCultivated())
						{
							base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
						}
					}
				}
				if (!this.HasEnoughLightToGrow)
				{
					this.unlitTicks += 2000;
				}
				else
				{
					this.unlitTicks = 0;
				}
				this.ageInt += 2000;
				if (this.Dying)
				{
					Map map = base.Map;
					bool isCrop = this.IsCrop;
					bool harvestableNow = this.HarvestableNow;
					bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
					int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
					base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					if (base.Destroyed)
					{
						if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
						{
							string key;
							if (harvestableNow)
							{
								key = "MessagePlantDiedOfRot_LeftUnharvested";
							}
							else if (dyingBecauseExposedToLight)
							{
								key = "MessagePlantDiedOfRot_ExposedToLight";
							}
							else
							{
								key = "MessagePlantDiedOfRot";
							}
							Messages.Message(key.Translate(new object[]
							{
								this.GetCustomLabelNoCount(false)
							}).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
						return;
					}
				}
				this.cachedLabelMouseover = null;
				if (this.def.plant.dropLeaves)
				{
					MoteLeaf moteLeaf = MoteMaker.MakeStaticMote(Vector3.zero, base.Map, ThingDefOf.Mote_Leaf, 1f) as MoteLeaf;
					if (moteLeaf != null)
					{
						float num3 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
						float treeHeight = this.def.graphicData.drawSize.x * num3;
						Vector3 b = Rand.InsideUnitCircleVec3 * Plant.LeafSpawnRadius;
						moteLeaf.Initialize(base.Position.ToVector3Shifted() + Vector3.up * Rand.Range(Plant.LeafSpawnYMin, Plant.LeafSpawnYMax) + b + Vector3.forward * this.def.graphicData.shadowData.offset.z, Rand.Value * 2000.TicksToSeconds(), b.z > 0f, treeHeight);
					}
				}
			}
		}

		protected virtual bool CurrentlyCultivated()
		{
			bool result;
			if (!this.def.plant.Sowable)
			{
				result = false;
			}
			else if (!base.Spawned)
			{
				result = false;
			}
			else
			{
				Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
				if (zone != null && zone is Zone_Growing)
				{
					result = true;
				}
				else
				{
					Building edifice = base.Position.GetEdifice(base.Map);
					result = (edifice != null && edifice.def.building.SupportsPlants);
				}
			}
			return result;
		}

		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		public virtual int YieldNow()
		{
			int result;
			if (!this.CanYieldNow())
			{
				result = 0;
			}
			else
			{
				float num = this.def.plant.harvestYield;
				float num2 = Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthInt);
				num2 = 0.5f + num2 * 0.5f;
				num *= num2;
				num *= Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints);
				num *= Find.Storyteller.difficulty.cropYieldFactor;
				result = GenMath.RoundRandom(num);
			}
			return result;
		}

		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			int num = Mathf.CeilToInt(this.growthInt * (float)this.def.plant.maxMeshCount);
			if (num < 1)
			{
				num = 1;
			}
			float num2 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
			float num3 = this.def.graphicData.drawSize.x * num2;
			Vector3 vector = Vector3.zero;
			int num4 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			bool flag = false;
			foreach (int num5 in positionIndices)
			{
				if (this.def.plant.maxMeshCount == 1)
				{
					vector = a + Gen.RandomHorizontalVector(0.05f);
					float num6 = (float)base.Position.z;
					if (vector.z - num2 / 2f < num6)
					{
						vector.z = num6 + num2 / 2f;
						flag = true;
					}
				}
				else
				{
					int num7 = 1;
					int maxMeshCount = this.def.plant.maxMeshCount;
					switch (maxMeshCount)
					{
					case 1:
						num7 = 1;
						break;
					default:
						if (maxMeshCount != 9)
						{
							if (maxMeshCount != 16)
							{
								if (maxMeshCount != 25)
								{
									Log.Error(this.def + " must have plant.MaxMeshCount that is a perfect square.", false);
								}
								else
								{
									num7 = 5;
								}
							}
							else
							{
								num7 = 4;
							}
						}
						else
						{
							num7 = 3;
						}
						break;
					case 4:
						num7 = 2;
						break;
					}
					float num8 = 1f / (float)num7;
					vector = base.Position.ToVector3();
					vector.y = this.def.Altitude;
					vector.x += 0.5f * num8;
					vector.z += 0.5f * num8;
					int num9 = num5 / num7;
					int num10 = num5 % num7;
					vector.x += (float)num9 * num8;
					vector.z += (float)num10 * num8;
					float max = num8 * 0.3f;
					vector += Gen.RandomHorizontalVector(max);
				}
				bool @bool = Rand.Bool;
				Material matSingle = this.Graphic.MatSingle;
				PlantUtility.SetWindExposureColors(Plant.workingColors, this);
				Vector2 vector2 = new Vector2(num3, num3);
				Vector3 center = vector;
				Vector2 size = vector2;
				Material mat = matSingle;
				bool flipUv = @bool;
				Printer_Plane.PrintPlane(layer, center, size, mat, 0f, flipUv, null, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
				num4++;
				if (num4 >= num)
				{
					break;
				}
			}
			if (this.def.graphicData.shadowData != null)
			{
				Vector3 center2 = a + this.def.graphicData.shadowData.offset * num2;
				if (flag)
				{
					center2.z = base.Position.ToVector3Shifted().z + this.def.graphicData.shadowData.offset.z;
				}
				center2.y -= 0.046875f;
				Vector3 volume = this.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center2, volume, Rot4.North);
			}
			Rand.PopState();
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.LifeStage == PlantLifeStage.Growing)
			{
				stringBuilder.AppendLine("PercentGrowth".Translate(new object[]
				{
					this.GrowthPercentString
				}));
				stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
				if (!this.Blighted)
				{
					if (this.Resting)
					{
						stringBuilder.AppendLine("PlantResting".Translate());
					}
					if (!this.HasEnoughLightToGrow)
					{
						stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + this.def.plant.growMinGlow.ToStringPercent());
					}
					float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
					if (growthRateFactor_Temperature < 0.99f)
					{
						if (growthRateFactor_Temperature < 0.01f)
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
						}
						else
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(new object[]
							{
								Mathf.RoundToInt(growthRateFactor_Temperature * 100f).ToString()
							}));
						}
					}
				}
			}
			else if (this.LifeStage == PlantLifeStage.Mature)
			{
				if (this.HarvestableNow)
				{
					stringBuilder.AppendLine("ReadyToHarvest".Translate());
				}
				else
				{
					stringBuilder.AppendLine("Mature".Translate());
				}
			}
			if (this.DyingBecauseExposedToLight)
			{
				stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
			}
			if (this.Blighted)
			{
				stringBuilder.AppendLine("Blighted".Translate() + " (" + this.Blight.Severity.ToStringPercent() + ")");
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return gizmo;
			}
			if (Prefs.DevMode && this.Blighted)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Spread blight",
					action = delegate()
					{
						this.Blight.TryReproduceNow();
					}
				};
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Plant()
		{
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<Gizmo> <GetGizmos>__BaseCallProxy0()
		{
			return base.GetGizmos();
		}

		public enum LeaflessCause
		{
			Cold,
			Poison
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <gizmo>__1;

			internal Command_Action <spread>__2;

			internal Plant $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<GetGizmos>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_126;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						gizmo = enumerator.Current;
						this.$current = gizmo;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (!Prefs.DevMode || !base.Blighted)
				{
					goto IL_126;
				}
				Command_Action spread = new Command_Action();
				spread.defaultLabel = "Dev: Spread blight";
				spread.action = delegate()
				{
					base.Blight.TryReproduceNow();
				};
				this.$current = spread;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_126:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Plant.<GetGizmos>c__Iterator0 <GetGizmos>c__Iterator = new Plant.<GetGizmos>c__Iterator0();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}

			internal void <>m__0()
			{
				base.Blight.TryReproduceNow();
			}
		}
	}
}
