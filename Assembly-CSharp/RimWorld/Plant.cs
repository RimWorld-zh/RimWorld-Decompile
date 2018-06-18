using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D7 RID: 1751
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x00144BC4 File Offset: 0x00142FC4
		// (set) Token: 0x060025FB RID: 9723 RVA: 0x00144BDF File Offset: 0x00142FDF
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

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x00144BF8 File Offset: 0x00142FF8
		// (set) Token: 0x060025FD RID: 9725 RVA: 0x00144C13 File Offset: 0x00143013
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

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060025FE RID: 9726 RVA: 0x00144C24 File Offset: 0x00143024
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x00144C6C File Offset: 0x0014306C
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

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002600 RID: 9728 RVA: 0x00144D48 File Offset: 0x00143148
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002601 RID: 9729 RVA: 0x00144D98 File Offset: 0x00143198
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

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002602 RID: 9730 RVA: 0x00144DE8 File Offset: 0x001431E8
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002603 RID: 9731 RVA: 0x00144E0C File Offset: 0x0014320C
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && !this.LeaflessNow && (!base.Spawned || base.Position.GetSnowDepth(base.Map) <= this.def.hideAtSnowDepth)));
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x00144EB8 File Offset: 0x001432B8
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

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002605 RID: 9733 RVA: 0x00144F94 File Offset: 0x00143394
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x00144FEC File Offset: 0x001433EC
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002607 RID: 9735 RVA: 0x00145010 File Offset: 0x00143410
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002608 RID: 9736 RVA: 0x00145048 File Offset: 0x00143448
		public virtual float GrowthRate
		{
			get
			{
				float result;
				if (this.Blighted)
				{
					result = 0f;
				}
				else if (base.Spawned && !GenPlant.GrowthSeasonNow(base.Position, base.Map, false))
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

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002609 RID: 9737 RVA: 0x001450B4 File Offset: 0x001434B4
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

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600260A RID: 9738 RVA: 0x00145110 File Offset: 0x00143510
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return base.Map.fertilityGrid.FertilityAt(base.Position) * this.def.plant.fertilitySensitivity + (1f - this.def.plant.fertilitySensitivity);
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x00145164 File Offset: 0x00143564
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

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x00145200 File Offset: 0x00143600
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

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x00145280 File Offset: 0x00143680
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

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x001452D8 File Offset: 0x001436D8
		protected string GrowthPercentString
		{
			get
			{
				return (this.growthInt + 0.0001f).ToStringPercent();
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x00145300 File Offset: 0x00143700
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

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x001453A8 File Offset: 0x001437A8
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x001453CC File Offset: 0x001437CC
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

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x00145410 File Offset: 0x00143810
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

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x001454C4 File Offset: 0x001438C4
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x001454FC File Offset: 0x001438FC
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				float num = 8f;
				return (float)this.HashOffset() * 0.01f % num - num + -2f;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x00145530 File Offset: 0x00143930
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

		// Token: 0x06002616 RID: 9750 RVA: 0x00145598 File Offset: 0x00143998
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad)
			{
				this.CheckTemperatureMakeLeafless();
			}
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x001455BC File Offset: 0x001439BC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x001455F0 File Offset: 0x001439F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x00145666 File Offset: 0x00143A66
		public override void PostMapInit()
		{
			this.CheckTemperatureMakeLeafless();
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x00145670 File Offset: 0x00143A70
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

		// Token: 0x0600261B RID: 9755 RVA: 0x00145700 File Offset: 0x00143B00
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

		// Token: 0x0600261C RID: 9756 RVA: 0x0014575E File Offset: 0x00143B5E
		protected virtual void CheckTemperatureMakeLeafless()
		{
			if (base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold);
			}
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x0014577C File Offset: 0x00143B7C
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
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
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
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
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

		// Token: 0x0600261E RID: 9758 RVA: 0x00145994 File Offset: 0x00143D94
		public override void TickLong()
		{
			this.CheckTemperatureMakeLeafless();
			if (!base.Destroyed)
			{
				if (GenPlant.GrowthSeasonNow(base.Position, base.Map, false))
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
					base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
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

		// Token: 0x0600261F RID: 9759 RVA: 0x00145CD0 File Offset: 0x001440D0
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

		// Token: 0x06002620 RID: 9760 RVA: 0x00145D78 File Offset: 0x00144178
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x00145DD4 File Offset: 0x001441D4
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

		// Token: 0x06002622 RID: 9762 RVA: 0x00145E78 File Offset: 0x00144278
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
				GenPlant.SetWindExposureColors(Plant.workingColors, this);
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

		// Token: 0x06002623 RID: 9763 RVA: 0x00146224 File Offset: 0x00144624
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

		// Token: 0x06002624 RID: 9764 RVA: 0x00146407 File Offset: 0x00144807
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x00146430 File Offset: 0x00144830
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

		// Token: 0x04001525 RID: 5413
		protected float growthInt = 0.05f;

		// Token: 0x04001526 RID: 5414
		protected int ageInt = 0;

		// Token: 0x04001527 RID: 5415
		protected int unlitTicks = 0;

		// Token: 0x04001528 RID: 5416
		protected int madeLeaflessTick = -99999;

		// Token: 0x04001529 RID: 5417
		public bool sown = false;

		// Token: 0x0400152A RID: 5418
		private string cachedLabelMouseover = null;

		// Token: 0x0400152B RID: 5419
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x0400152C RID: 5420
		public const float BaseGrowthPercent = 0.05f;

		// Token: 0x0400152D RID: 5421
		private const float BaseDyingDamagePerTick = 0.005f;

		// Token: 0x0400152E RID: 5422
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		// Token: 0x0400152F RID: 5423
		private const float GridPosRandomnessFactor = 0.3f;

		// Token: 0x04001530 RID: 5424
		private const int TicksWithoutLightBeforeStartDying = 450000;

		// Token: 0x04001531 RID: 5425
		private const int LeaflessMinRecoveryTicks = 60000;

		// Token: 0x04001532 RID: 5426
		public const float MinGrowthTemperature = 0f;

		// Token: 0x04001533 RID: 5427
		public const float MinOptimalGrowthTemperature = 10f;

		// Token: 0x04001534 RID: 5428
		public const float MaxOptimalGrowthTemperature = 42f;

		// Token: 0x04001535 RID: 5429
		public const float MaxGrowthTemperature = 58f;

		// Token: 0x04001536 RID: 5430
		public const float MaxLeaflessTemperature = -2f;

		// Token: 0x04001537 RID: 5431
		private const float MinLeaflessTemperature = -10f;

		// Token: 0x04001538 RID: 5432
		private const float MinAnimalEatPlantsTemperature = 0f;

		// Token: 0x04001539 RID: 5433
		public const float TopVerticesAltitudeBias = 0.1f;

		// Token: 0x0400153A RID: 5434
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		// Token: 0x0400153B RID: 5435
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		// Token: 0x0400153C RID: 5436
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		// Token: 0x0400153D RID: 5437
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		// Token: 0x020006D8 RID: 1752
		public enum LeaflessCause
		{
			// Token: 0x0400153F RID: 5439
			Cold,
			// Token: 0x04001540 RID: 5440
			Poison
		}
	}
}
