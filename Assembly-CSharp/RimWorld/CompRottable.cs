using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompRottable : ThingComp
	{
		private float rotProgressInt;

		private CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)base.props;
			}
		}

		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

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

		public RotStage Stage
		{
			get
			{
				if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
				{
					return RotStage.Fresh;
				}
				if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
				{
					return RotStage.Rotting;
				}
				return RotStage.Dessicated;
			}
		}

		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float ambientTemperature = base.parent.AmbientTemperature;
				ambientTemperature = (float)Mathf.RoundToInt(ambientTemperature);
				return this.TicksUntilRotAtTemp(ambientTemperature);
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
		}

		public override void CompTickRare()
		{
			float rotProgress = this.RotProgress;
			float num = 1f;
			float ambientTemperature = base.parent.AmbientTemperature;
			num *= GenTemperature.RotRateAtTemperature(ambientTemperature);
			this.RotProgress += Mathf.Round((float)(num * 250.0));
			if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
			{
				if (base.parent.Spawned && base.parent.Map.slotGroupManager.SlotGroupAt(base.parent.Position) != null)
				{
					Messages.Message("MessageRottedAwayInStorage".Translate(base.parent.Label).CapitalizeFirst(), MessageSound.Silent);
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
				}
				base.parent.Destroy(DestroyMode.Vanish);
			}
			else if (Mathf.FloorToInt((float)(rotProgress / 60000.0)) != Mathf.FloorToInt((float)(this.RotProgress / 60000.0)) && this.ShouldTakeRotDamage())
			{
				if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0.0)
				{
					base.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
				else if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0.0)
				{
					base.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				}
			}
		}

		private bool ShouldTakeRotDamage()
		{
			if (base.parent.ParentHolder != null)
			{
				Thing thing = base.parent.ParentHolder as Thing;
				if (thing != null && thing.def.category == ThingCategory.Building && thing.def.building.preventDeterioration)
				{
					return false;
				}
			}
			return true;
		}

		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(base.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != 0)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, base.parent);
			}
		}

		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.Stage)
			{
			case RotStage.Fresh:
			{
				stringBuilder.Append("RotStateFresh".Translate() + ".");
				break;
			}
			case RotStage.Rotting:
			{
				stringBuilder.Append("RotStateRotting".Translate() + ".");
				break;
			}
			case RotStage.Dessicated:
			{
				stringBuilder.Append("RotStateDessicated".Translate() + ".");
				break;
			}
			}
			float num = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
			if (num > 0.0)
			{
				float ambientTemperature = base.parent.AmbientTemperature;
				ambientTemperature = (float)Mathf.RoundToInt(ambientTemperature);
				float num2 = GenTemperature.RotRateAtTemperature(ambientTemperature);
				int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
				stringBuilder.AppendLine();
				if (num2 < 0.0010000000474974513)
				{
					stringBuilder.Append("CurrentlyFrozen".Translate() + ".");
				}
				else if (num2 < 0.99900001287460327)
				{
					stringBuilder.Append("CurrentlyRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriodVagueMax()) + ".");
				}
				else
				{
					stringBuilder.Append("NotRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriodVagueMax()) + ".");
				}
			}
			return stringBuilder.ToString();
		}

		public int ApproxTicksUntilRotWhenAtTempOfTile(int tile)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(Find.TickManager.TicksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		public int TicksUntilRotAtTemp(float temp)
		{
			float num = GenTemperature.RotRateAtTemperature(temp);
			if (num <= 0.0)
			{
				return 2147483647;
			}
			float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
			if (num2 <= 0.0)
			{
				return 0;
			}
			return Mathf.RoundToInt(num2 / num);
		}

		private void StageChanged()
		{
			Corpse corpse = base.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}
	}
}
