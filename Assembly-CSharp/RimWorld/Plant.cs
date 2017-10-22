using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		public enum LeaflessCause
		{
			Cold = 0,
			Poison = 1
		}

		protected float growthInt = 0.05f;

		protected int ageInt = 0;

		protected int unlitTicks = 0;

		protected int madeLeaflessTick = -99999;

		public bool sown = false;

		private string cachedLabelMouseover = (string)null;

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

		public const float SeedShootMinGrowthPercent = 0.6f;

		public const float TopVerticesAltitudeBias = 0.1f;

		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = Mathf.Clamp01(value);
				this.cachedLabelMouseover = (string)null;
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
				this.cachedLabelMouseover = (string)null;
			}
		}

		public virtual bool HarvestableNow
		{
			get
			{
				return base.def.plant.Harvestable && this.growthInt > base.def.plant.harvestMinGrowth;
			}
		}

		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && base.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing;
			}
		}

		public Blight Blight
		{
			get
			{
				return (base.Spawned && base.def.plant.Blightable) ? base.Position.GetFirstBlight(base.Map) : null;
			}
		}

		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		public virtual bool CanReproduceNow
		{
			get
			{
				return base.Spawned && base.def.plant.reproduces && this.growthInt >= 0.60000002384185791 && GenPlant.SnowAllowsPlanting(base.Position, base.Map) && !this.Blighted;
			}
		}

		public override bool IngestibleNow
		{
			get
			{
				return (byte)(base.IngestibleNow ? (base.def.plant.IsTree ? 1 : ((!(this.growthInt < base.def.plant.harvestMinGrowth)) ? ((!this.LeaflessNow) ? ((!base.Spawned || !(base.Position.GetSnowDepth(base.Map) > base.def.hideAtSnowDepth)) ? 1 : 0) : 0) : 0)) : 0) != 0;
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
					if (base.def.plant.LimitedLifespan && this.ageInt > base.def.plant.LifespanTicks)
					{
						num = Mathf.Max(num, 0.005f);
					}
					if (!base.def.plant.cavePlant && this.unlitTicks > 450000)
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
				return base.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0.0;
			}
		}

		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0.0;
			}
		}

		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25 || GenLocalDate.DayPercent(this) > 0.800000011920929;
			}
		}

		public virtual float GrowthRate
		{
			get
			{
				return (float)((!this.Blighted) ? (this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light) : 0.0);
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
					float num = (float)(1.0 / (60000.0 * base.def.plant.growDays));
					result = num * this.GrowthRate;
				}
				return result;
			}
		}

		public float GrowthRateFactor_Fertility
		{
			get
			{
				return (float)(base.Map.fertilityGrid.FertilityAt(base.Position) * base.def.plant.fertilitySensitivity + (1.0 - base.def.plant.fertilitySensitivity));
			}
		}

		public float GrowthRateFactor_Light
		{
			get
			{
				float num = base.Map.glowGrid.GameGlowAt(base.Position, false);
				return (float)((base.def.plant.growMinGlow != base.def.plant.growOptimalGlow || num != base.def.plant.growOptimalGlow) ? GenMath.InverseLerp(base.def.plant.growMinGlow, base.def.plant.growOptimalGlow, num) : 1.0);
			}
		}

		public float GrowthRateFactor_Temperature
		{
			get
			{
				float num = default(float);
				return (float)(GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num) ? ((!(num < 10.0)) ? ((!(num > 42.0)) ? 1.0 : Mathf.InverseLerp(58f, 42f, num)) : Mathf.InverseLerp(0f, 10f, num)) : 1.0);
			}
		}

		protected int TicksUntilFullyGrown
		{
			get
			{
				int result;
				if (this.growthInt > 0.99989998340606689)
				{
					result = 0;
				}
				else
				{
					float growthPerTick = this.GrowthPerTick;
					result = ((growthPerTick != 0.0) ? ((int)((1.0 - this.growthInt) / growthPerTick)) : 2147483647);
				}
				return result;
			}
		}

		protected string GrowthPercentString
		{
			get
			{
				return ((float)(this.growthInt + 9.9999997473787516E-05)).ToStringPercent();
			}
		}

		public override string LabelMouseover
		{
			get
			{
				if (this.cachedLabelMouseover == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(base.def.LabelCap);
					stringBuilder.Append(" (" + "PercentGrowth".Translate(this.GrowthPercentString));
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
				return this.GrowthRateFactor_Light > 0.0010000000474974513;
			}
		}

		public virtual PlantLifeStage LifeStage
		{
			get
			{
				return (PlantLifeStage)((!(this.growthInt < 0.0010000000474974513)) ? ((!(this.growthInt > 0.99900001287460327)) ? 1 : 2) : 0);
			}
		}

		public override Graphic Graphic
		{
			get
			{
				return (this.LifeStage != 0) ? ((base.def.plant.leaflessGraphic == null || !this.LeaflessNow || (this.sown && this.HarvestableNow)) ? ((base.def.plant.immatureGraphic == null || this.HarvestableNow) ? base.Graphic : base.def.plant.immatureGraphic) : base.def.plant.leaflessGraphic) : Plant.GraphicSowing;
			}
		}

		public bool LeaflessNow
		{
			get
			{
				return (byte)((Find.TickManager.TicksGame - this.madeLeaflessTick < 60000) ? 1 : 0) != 0;
			}
		}

		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)((float)this.HashOffset() * 0.0099999997764825821 % num - num + -2.0);
			}
		}

		public bool IsCrop
		{
			get
			{
				bool result;
				if (!base.def.plant.Sowable)
				{
					result = false;
				}
				else if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.");
					result = false;
				}
				else
				{
					result = (base.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map));
				}
				return result;
			}
		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		public override void DeSpawn()
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn();
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
			float nutrition = base.def.ingestible.nutrition;
			nutrition = ((!base.def.plant.Sowable) ? (nutrition * Mathf.Lerp(0.5f, 1f, this.growthInt)) : (nutrition * this.growthInt));
			if (base.def.plant.HarvestDestroys)
			{
				numTaken = 1;
			}
			else
			{
				this.growthInt -= 0.3f;
				if (this.growthInt < 0.079999998211860657)
				{
					this.growthInt = 0.08f;
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				numTaken = 0;
			}
			nutritionIngested = nutrition;
		}

		public virtual void PlantCollected()
		{
			if (base.def.plant.HarvestDestroys)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				this.growthInt = base.def.plant.harvestAfterGrowth;
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(LeaflessCause.Cold);
			}
		}

		public virtual void MakeLeafless(LeaflessCause cause)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			if (cause == LeaflessCause.Poison && base.def.plant.leaflessGraphic == null)
			{
				if (this.IsCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + base.def.defName, 240f))
				{
					Messages.Message("MessagePlantDiedOfPoison".Translate(this.Label).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent);
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
			}
			else if (base.def.plant.dieIfLeafless)
			{
				if (this.IsCrop)
				{
					switch (cause)
					{
					case LeaflessCause.Cold:
					{
						if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + base.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfCold".Translate(this.Label).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent);
						}
						break;
					}
					case LeaflessCause.Poison:
					{
						if (MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + base.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfPoison".Translate(this.Label).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent);
						}
						break;
					}
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
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
				if (GenPlant.GrowthSeasonNow(base.Position, base.Map))
				{
					float num = this.growthInt;
					bool flag = this.LifeStage == PlantLifeStage.Mature;
					this.growthInt += (float)(this.GrowthPerTick * 2000.0);
					if (this.growthInt > 1.0)
					{
						this.growthInt = 1f;
					}
					if (!flag && this.LifeStage == PlantLifeStage.Mature)
					{
						goto IL_009f;
					}
					if ((int)(num * 10.0) != (int)(this.growthInt * 10.0))
						goto IL_009f;
					goto IL_00c3;
				}
				goto IL_010e;
			}
			return;
			IL_010e:
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
				int amount = Mathf.CeilToInt((float)(this.CurrentDyingDamagePerTick * 2000.0));
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				if (base.Destroyed)
				{
					if (isCrop && base.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + base.def.defName, 240f))
					{
						string key = (!harvestableNow) ? ((!dyingBecauseExposedToLight) ? "MessagePlantDiedOfRot" : "MessagePlantDiedOfRot_ExposedToLight") : "MessagePlantDiedOfRot_LeftUnharvested";
						Messages.Message(key.Translate(this.Label).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent);
					}
					return;
				}
			}
			this.cachedLabelMouseover = (string)null;
			return;
			IL_00c3:
			if (this.CanReproduceNow && Rand.MTBEventOccurs(base.def.plant.reproduceMtbDays, 60000f, 2000f))
			{
				GenPlantReproduction.TryReproduceFrom(base.Position, base.def, SeedTargFindMode.Reproduce, base.Map);
			}
			goto IL_010e;
			IL_009f:
			if (this.CurrentlyCultivated())
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
			goto IL_00c3;
		}

		protected virtual bool CurrentlyCultivated()
		{
			bool result;
			if (!base.def.plant.Sowable)
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
					result = ((byte)((edifice != null && edifice.def.building.SupportsPlants) ? 1 : 0) != 0);
				}
			}
			return result;
		}

		public virtual int YieldNow()
		{
			int result;
			if (!this.HarvestableNow)
			{
				result = 0;
			}
			else if (base.def.plant.harvestYield <= 0.0)
			{
				result = 0;
			}
			else if (this.Blighted)
			{
				result = 0;
			}
			else
			{
				float harvestYield = base.def.plant.harvestYield;
				float num = Mathf.InverseLerp(base.def.plant.harvestMinGrowth, 1f, this.growthInt);
				num = (float)(0.5 + num * 0.5);
				harvestYield *= num;
				harvestYield *= Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints);
				harvestYield *= Find.Storyteller.difficulty.cropYieldFactor;
				result = GenMath.RoundRandom(harvestYield);
			}
			return result;
		}

		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			int num = Mathf.CeilToInt(this.growthInt * (float)base.def.plant.maxMeshCount);
			if (num < 1)
			{
				num = 1;
			}
			float num2 = base.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
			float num3 = base.def.graphicData.drawSize.x * num2;
			Vector3 vector = Vector3.zero;
			int num4 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			bool flag = false;
			int num5 = 0;
			while (num5 < positionIndices.Length)
			{
				int num6 = positionIndices[num5];
				if (base.def.plant.maxMeshCount == 1)
				{
					vector = a + Gen.RandomHorizontalVector(0.05f);
					IntVec3 position = base.Position;
					float num7 = (float)position.z;
					if (vector.z - num2 / 2.0 < num7)
					{
						vector.z = (float)(num7 + num2 / 2.0);
						flag = true;
					}
				}
				else
				{
					int num8 = 1;
					switch (base.def.plant.maxMeshCount)
					{
					case 1:
					{
						num8 = 1;
						break;
					}
					case 4:
					{
						num8 = 2;
						break;
					}
					case 9:
					{
						num8 = 3;
						break;
					}
					case 16:
					{
						num8 = 4;
						break;
					}
					case 25:
					{
						num8 = 5;
						break;
					}
					default:
					{
						Log.Error(base.def + " must have plant.MaxMeshCount that is a perfect square.");
						break;
					}
					}
					float num9 = (float)(1.0 / (float)num8);
					vector = base.Position.ToVector3();
					vector.y = base.def.Altitude;
					vector.x += (float)(0.5 * num9);
					vector.z += (float)(0.5 * num9);
					int num10 = num6 / num8;
					int num11 = num6 % num8;
					vector.x += (float)num10 * num9;
					vector.z += (float)num11 * num9;
					float max = (float)(num9 * 0.30000001192092896);
					vector += Gen.RandomHorizontalVector(max);
				}
				bool @bool = Rand.Bool;
				Material matSingle = this.Graphic.MatSingle;
				GenPlant.SetWindExposureColors(Plant.workingColors, this);
				Vector2 vector2 = new Vector2(num3, num3);
				Vector3 center = vector;
				Vector2 size = vector2;
				Material mat = matSingle;
				bool flipUv = @bool;
				Printer_Plane.PrintPlane(layer, center, size, mat, 0f, flipUv, null, Plant.workingColors, 0.1f);
				num4++;
				if (num4 < num)
				{
					num5++;
					continue;
				}
				break;
			}
			if (base.def.graphicData.shadowData != null)
			{
				Vector3 center2 = a + base.def.graphicData.shadowData.offset * num2;
				if (flag)
				{
					Vector3 center = base.Position.ToVector3Shifted();
					center2.z = center.z + base.def.graphicData.shadowData.offset.z;
				}
				center2.y -= 0.046875f;
				Vector3 volume = base.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center2, volume, Rot4.North);
			}
			Rand.PopState();
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.LifeStage == PlantLifeStage.Growing)
			{
				stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
				stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
				if (!this.Blighted)
				{
					if (this.Resting)
					{
						stringBuilder.AppendLine("PlantResting".Translate());
					}
					if (!this.HasEnoughLightToGrow)
					{
						stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + base.def.plant.growMinGlow.ToStringPercent());
					}
					float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
					if (growthRateFactor_Temperature < 0.99000000953674316)
					{
						if (growthRateFactor_Temperature < 0.0099999997764825821)
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
						}
						else
						{
							stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(Mathf.RoundToInt((float)(growthRateFactor_Temperature * 100.0)).ToString()));
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
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map);
			}
		}
	}
}
