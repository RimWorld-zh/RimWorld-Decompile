using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Hediff_Addiction : HediffWithComps
	{
		private const int DefaultStageIndex = 0;

		private const int WithdrawalStageIndex = 1;

		public Need_Chemical Need
		{
			get
			{
				if (base.pawn.Dead)
				{
					return null;
				}
				List<Need> allNeeds = base.pawn.needs.AllNeeds;
				for (int i = 0; i < allNeeds.Count; i++)
				{
					if (allNeeds[i].def == base.def.causesNeed)
					{
						return (Need_Chemical)allNeeds[i];
					}
				}
				return null;
			}
		}

		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].addictionHediff == base.def)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			}
		}

		public override string LabelInBrackets
		{
			get
			{
				if (this.CurStageIndex == 1 && base.def.CompProps<HediffCompProperties_SeverityPerDay>() != null)
				{
					return base.LabelInBrackets + " " + ((float)(1.0 - this.Severity)).ToStringPercent();
				}
				return base.LabelInBrackets;
			}
		}

		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need != null && need.CurCategory == DrugDesireCategory.Withdrawal)
				{
					return 1;
				}
				return 0;
			}
		}

		public void Notify_NeedCategoryChanged()
		{
			base.pawn.health.Notify_HediffChanged(this);
		}
	}
}
