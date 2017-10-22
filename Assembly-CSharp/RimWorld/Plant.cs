using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Plant : Thing
	{
		public const float BaseGrowthPercent = 0.05f;

		private const float DyingDamagePerTick = 0.005f;

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

		protected float growthInt = 0.05f;

		protected int ageInt;

		protected int unlitTicks;

		protected int madeLeaflessTick = -99999;

		public bool sown;

		private string cachedLabelMouseover;

		private static Color32[] workingColors = new Color32[4];

		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = value;
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

		public override bool IngestibleNow
		{
			get
			{
				if (!base.IngestibleNow)
				{
					return false;
				}
				if (base.def.plant.IsTree)
				{
					return true;
				}
				if (this.growthInt < base.def.plant.harvestMinGrowth)
				{
					return false;
				}
				if (this.LeaflessNow)
				{
					return false;
				}
				if (base.Spawned && base.Position.GetSnowDepth(base.Map) > base.def.hideAtSnowDepth)
				{
					return false;
				}
				return true;
			}
		}

		public virtual bool Dying
		{
			get
			{
				if (base.def.plant.LimitedLifespan && this.ageInt > base.def.plant.LifespanTicks)
				{
					return true;
				}
				if (this.unlitTicks > 450000)
				{
					return true;
				}
				return false;
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
				return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light;
			}
		}

		protected float GrowthPerTick
		{
			get
			{
				if (this.LifeStage == PlantLifeStage.Growing && !this.Resting)
				{
					float num = (float)(1.0 / (60000.0 * base.def.plant.growDays));
					return num * this.GrowthRate;
				}
				return 0f;
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
				float value = Mathf.InverseLerp(base.def.plant.growMinGlow, base.def.plant.growOptimalGlow, base.Map.glowGrid.GameGlowAt(base.Position));
				return Mathf.Clamp01(value);
			}
		}

		public float GrowthRateFactor_Temperature
		{
			get
			{
				float num = default(float);
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
				{
					return 1f;
				}
				if (num < 10.0)
				{
					return Mathf.InverseLerp(0f, 10f, num);
				}
				if (num > 42.0)
				{
					return Mathf.InverseLerp(58f, 42f, num);
				}
				return 1f;
			}
		}

		protected int TicksUntilFullyGrown
		{
			get
			{
				if (this.growthInt > 0.99989998340606689)
				{
					return 0;
				}
				float growthPerTick = this.GrowthPerTick;
				if (growthPerTick == 0.0)
				{
					return 2147483647;
				}
				return (int)((1.0 - this.growthInt) / growthPerTick);
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
				if (this.growthInt < 0.0010000000474974513)
				{
					return PlantLifeStage.Sowing;
				}
				if (this.growthInt > 0.99900001287460327)
				{
					return PlantLifeStage.Mature;
				}
				return PlantLifeStage.Growing;
			}
		}

		public override Graphic Graphic
		{
			get
			{
				if (this.LifeStage == PlantLifeStage.Sowing)
				{
					return Plant.GraphicSowing;
				}
				if (base.def.plant.leaflessGraphic != null && this.LeaflessNow && !this.HarvestableNow)
				{
					return base.def.plant.leaflessGraphic;
				}
				if (base.def.plant.immatureGraphic != null && !this.HarvestableNow)
				{
					return base.def.plant.immatureGraphic;
				}
				return base.Graphic;
			}
		}

		public bool LeaflessNow
		{
			get
			{
				if (Find.TickManager.TicksGame - this.madeLeaflessTick < 60000)
				{
					return true;
				}
				return false;
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
				if (!base.def.plant.Sowable)
				{
					return false;
				}
				if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.");
					return false;
				}
				return base.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map);
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

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
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
				this.MakeLeafless(true);
			}
		}

		public virtual void MakeLeafless(bool causedByCold = false)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			this.madeLeaflessTick = Find.TickManager.TicksGame;
			if (base.def.plant.dieIfLeafless)
			{
				if (causedByCold && this.IsCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + base.def.defName, 240f))
				{
					Messages.Message("MessagePlantDiedOfCold".Translate(this.Label).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageSound.Negative);
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
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
					if (!this.HasEnoughLightToGrow)
					{
						this.unlitTicks += 2000;
					}
					else
					{
						this.unlitTicks = 0;
					}
					float num = this.growthInt;
					bool flag = this.LifeStage == PlantLifeStage.Mature;
					this.growthInt += (float)(this.GrowthPerTick * 2000.0);
					if (this.growthInt > 1.0)
					{
						this.growthInt = 1f;
					}
					if (!flag && this.LifeStage == PlantLifeStage.Mature)
					{
						goto IL_00c4;
					}
					if ((int)(num * 10.0) != (int)(this.growthInt * 10.0))
						goto IL_00c4;
					goto IL_00e6;
				}
				goto IL_0249;
			}
			return;
			IL_0249:
			this.cachedLabelMouseover = (string)null;
			return;
			IL_00c4:
			if (this.CurrentlyCultivated())
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
			goto IL_00e6;
			IL_00e6:
			if (base.def.plant.LimitedLifespan)
			{
				this.ageInt += 2000;
				if (this.Dying)
				{
					Map map = base.Map;
					bool isCrop = this.IsCrop;
					int amount = Mathf.CeilToInt(10f);
					base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
					if (base.Destroyed)
					{
						if (isCrop && base.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + base.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfRot".Translate(this.Label).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageSound.Negative);
						}
						return;
					}
				}
			}
			if (base.def.plant.reproduces && this.growthInt >= 0.60000002384185791 && Rand.MTBEventOccurs(base.def.plant.reproduceMtbDays, 60000f, 2000f))
			{
				if (!GenPlant.SnowAllowsPlanting(base.Position, base.Map))
				{
					return;
				}
				GenPlantReproduction.TryReproduceFrom(base.Position, base.def, SeedTargFindMode.Reproduce, base.Map);
			}
			goto IL_0249;
		}

		protected virtual bool CurrentlyCultivated()
		{
			if (!base.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
			if (zone != null && zone is Zone_Growing)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			if (edifice != null && edifice.def.building.SupportsPlants)
			{
				return true;
			}
			return false;
		}

		public virtual int YieldNow()
		{
			if (!this.HarvestableNow)
			{
				return 0;
			}
			if (base.def.plant.harvestYield <= 0.0)
			{
				return 0;
			}
			float harvestYield = base.def.plant.harvestYield;
			float num = Mathf.InverseLerp(base.def.plant.harvestMinGrowth, 1f, this.growthInt);
			num = (float)(0.5 + num * 0.5);
			harvestYield *= num;
			harvestYield *= Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints);
			harvestYield *= Find.Storyteller.difficulty.cropYieldFactor;
			return GenMath.RoundRandom(harvestYield);
		}

		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			float num = (float)((base.def.plant.maxMeshCount != 1) ? 0.5 : 0.05000000074505806);
			int num2 = Mathf.CeilToInt(this.growthInt * (float)base.def.plant.maxMeshCount);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = 1;
			switch (base.def.plant.maxMeshCount)
			{
			case 1:
			{
				num3 = 1;
				break;
			}
			case 4:
			{
				num3 = 2;
				break;
			}
			case 9:
			{
				num3 = 3;
				break;
			}
			case 16:
			{
				num3 = 4;
				break;
			}
			case 25:
			{
				num3 = 5;
				break;
			}
			default:
			{
				Log.Error(base.def + " must have plant.MaxMeshCount that is a perfect square.");
				break;
			}
			}
			float num4 = (float)(1.0 / (float)num3);
			Vector3 vector = Vector3.zero;
			Vector2 size = Vector2.zero;
			int num5 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			int num6 = 0;
			while (num6 < positionIndices.Length)
			{
				int num7 = positionIndices[num6];
				float num8 = base.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
				if (base.def.plant.maxMeshCount == 1)
				{
					vector = a + new Vector3(Rand.Range((float)(0.0 - num), num), 0f, Rand.Range((float)(0.0 - num), num));
					float num9 = Mathf.Floor(a.z);
					if (vector.z - num8 / 2.0 < num9)
					{
						vector.z = (float)(num9 + num8 / 2.0);
					}
				}
				else
				{
					vector = base.Position.ToVector3();
					vector.y = base.def.Altitude;
					vector.x += (float)(0.5 * num4);
					vector.z += (float)(0.5 * num4);
					int num10 = num7 / num3;
					int num11 = num7 % num3;
					vector.x += (float)num10 * num4;
					vector.z += (float)num11 * num4;
					float num12 = (float)(num4 * 0.30000001192092896);
					vector += new Vector3(Rand.Range((float)(0.0 - num12), num12), 0f, Rand.Range((float)(0.0 - num12), num12));
				}
				bool flag = Rand.Value < 0.5;
				Material matSingle = this.Graphic.MatSingle;
				Plant.workingColors[1].a = (Plant.workingColors[2].a = (byte)(255.0 * base.def.plant.topWindExposure));
				Plant.workingColors[0].a = (Plant.workingColors[3].a = (byte)0);
				num8 *= base.def.graphicData.drawSize.x;
				size = new Vector2(num8, num8);
				bool flipUv = flag;
				Printer_Plane.PrintPlane(layer, vector, size, matSingle, 0f, flipUv, null, Plant.workingColors, 0.1f);
				num5++;
				if (num5 < num2)
				{
					num6++;
					continue;
				}
				break;
			}
			if (base.def.graphicData.shadowData != null)
			{
				float num13 = 0.85f;
				num13 = (float)((!(size.y < 1.0)) ? 0.81000000238418579 : 0.60000002384185791);
				Vector3 center = vector;
				center.z -= (float)(size.y / 2.0 * num13);
				center.y -= 0.046875f;
				Printer_Shadow.PrintShadow(layer, center, base.def.graphicData.shadowData, Rot4.North);
			}
			Rand.PopState();
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.LifeStage == PlantLifeStage.Growing)
			{
				stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
				stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
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
			else if (this.LifeStage == PlantLifeStage.Mature)
			{
				if (base.def.plant.Harvestable)
				{
					stringBuilder.AppendLine("ReadyToHarvest".Translate());
				}
				else
				{
					stringBuilder.AppendLine("Mature".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		public virtual void CropBlighted()
		{
			if (Rand.Value < 0.85000002384185791)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
