using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000733 RID: 1843
	public class CompRottable : ThingComp
	{
		// Token: 0x04001646 RID: 5702
		private float rotProgressInt = 0f;

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x0015B0F4 File Offset: 0x001594F4
		public CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)this.props;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x0015B114 File Offset: 0x00159514
		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x0015B13C File Offset: 0x0015953C
		// (set) Token: 0x0600289D RID: 10397 RVA: 0x0015B158 File Offset: 0x00159558
		public float RotProgress
		{
			get
			{
				return this.rotProgressInt;
			}
			set
			{
				RotStage stage = this.Stage;
				this.rotProgressInt = value;
				if (stage != this.Stage)
				{
					this.StageChanged();
				}
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x0015B188 File Offset: 0x00159588
		public RotStage Stage
		{
			get
			{
				RotStage result;
				if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
				{
					result = RotStage.Fresh;
				}
				else if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
				{
					result = RotStage.Rotting;
				}
				else
				{
					result = RotStage.Dessicated;
				}
				return result;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x0015B1DC File Offset: 0x001595DC
		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float num = this.parent.AmbientTemperature;
				num = (float)Mathf.RoundToInt(num);
				return this.TicksUntilRotAtTemp(num);
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x0015B20C File Offset: 0x0015960C
		public bool Active
		{
			get
			{
				if (this.PropsRot.disableIfHatcher)
				{
					CompHatcher compHatcher = this.parent.TryGetComp<CompHatcher>();
					if (compHatcher != null && !compHatcher.TemperatureDamaged)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x0015B258 File Offset: 0x00159658
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0015B277 File Offset: 0x00159677
		public override void CompTick()
		{
			this.Tick(1);
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0015B281 File Offset: 0x00159681
		public override void CompTickRare()
		{
			this.Tick(250);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0015B290 File Offset: 0x00159690
		private void Tick(int interval)
		{
			if (this.Active)
			{
				float rotProgress = this.RotProgress;
				float ambientTemperature = this.parent.AmbientTemperature;
				float num = GenTemperature.RotRateAtTemperature(ambientTemperature);
				this.RotProgress += num * (float)interval;
				if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
				{
					if (this.parent.IsInAnyStorage() && this.parent.SpawnedOrAnyParentSpawned)
					{
						Messages.Message("MessageRottedAwayInStorage".Translate(new object[]
						{
							this.parent.Label
						}).CapitalizeFirst(), new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false), MessageTypeDefOf.NegativeEvent, true);
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
					}
					this.parent.Destroy(DestroyMode.Vanish);
				}
				else
				{
					bool flag = Mathf.FloorToInt(rotProgress / 60000f) != Mathf.FloorToInt(this.RotProgress / 60000f);
					if (flag && this.ShouldTakeRotDamage())
					{
						if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0f)
						{
							this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
						}
						else if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
						{
							this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
						}
					}
				}
			}
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x0015B464 File Offset: 0x00159864
		private bool ShouldTakeRotDamage()
		{
			Thing thing = this.parent.ParentHolder as Thing;
			return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0015B4C0 File Offset: 0x001598C0
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x0015B504 File Offset: 0x00159904
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x0015B51D File Offset: 0x0015991D
		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != RotStage.Fresh)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent);
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0015B538 File Offset: 0x00159938
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Active)
			{
				result = null;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				RotStage stage = this.Stage;
				if (stage != RotStage.Fresh)
				{
					if (stage != RotStage.Rotting)
					{
						if (stage == RotStage.Dessicated)
						{
							stringBuilder.Append("RotStateDessicated".Translate() + ".");
						}
					}
					else
					{
						stringBuilder.Append("RotStateRotting".Translate() + ".");
					}
				}
				else
				{
					stringBuilder.Append("RotStateFresh".Translate() + ".");
				}
				float num = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
				if (num > 0f)
				{
					float num2 = this.parent.AmbientTemperature;
					num2 = (float)Mathf.RoundToInt(num2);
					float num3 = GenTemperature.RotRateAtTemperature(num2);
					int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
					stringBuilder.AppendLine();
					if (num3 < 0.001f)
					{
						stringBuilder.Append("CurrentlyFrozen".Translate() + ".");
					}
					else if (num3 < 0.999f)
					{
						stringBuilder.Append("CurrentlyRefrigerated".Translate(new object[]
						{
							ticksUntilRotAtCurrentTemp.ToStringTicksToPeriodVague(true, true)
						}) + ".");
					}
					else
					{
						stringBuilder.Append("NotRefrigerated".Translate(new object[]
						{
							ticksUntilRotAtCurrentTemp.ToStringTicksToPeriodVague(true, true)
						}) + ".");
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x0015B6D8 File Offset: 0x00159AD8
		public int ApproxTicksUntilRotWhenAtTempOfTile(int tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x0015B6FC File Offset: 0x00159AFC
		public int TicksUntilRotAtTemp(float temp)
		{
			int result;
			if (!this.Active)
			{
				result = 72000000;
			}
			else
			{
				float num = GenTemperature.RotRateAtTemperature(temp);
				if (num <= 0f)
				{
					result = 72000000;
				}
				else
				{
					float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
					if (num2 <= 0f)
					{
						result = 0;
					}
					else
					{
						result = Mathf.RoundToInt(num2 / num);
					}
				}
			}
			return result;
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x0015B774 File Offset: 0x00159B74
		private void StageChanged()
		{
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x0015B79A File Offset: 0x00159B9A
		public void RotImmediately()
		{
			if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
			{
				this.RotProgress = (float)this.PropsRot.TicksToRotStart;
			}
		}
	}
}
