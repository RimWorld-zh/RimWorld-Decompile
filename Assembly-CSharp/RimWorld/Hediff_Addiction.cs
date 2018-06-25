using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Hediff_Addiction : HediffWithComps
	{
		private const int DefaultStageIndex = 0;

		private const int WithdrawalStageIndex = 1;

		public Hediff_Addiction()
		{
		}

		public Need_Chemical Need
		{
			get
			{
				Need_Chemical result;
				if (this.pawn.Dead)
				{
					result = null;
				}
				else
				{
					List<Need> allNeeds = this.pawn.needs.AllNeeds;
					for (int i = 0; i < allNeeds.Count; i++)
					{
						if (allNeeds[i].def == this.def.causesNeed)
						{
							return (Need_Chemical)allNeeds[i];
						}
					}
					result = null;
				}
				return result;
			}
		}

		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].addictionHediff == this.def)
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
				string result;
				if (this.CurStageIndex == 1 && this.def.CompProps<HediffCompProperties_SeverityPerDay>() != null)
				{
					result = base.LabelInBrackets + " " + (1f - this.Severity).ToStringPercent();
				}
				else
				{
					result = base.LabelInBrackets;
				}
				return result;
			}
		}

		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				int result;
				if (need == null || need.CurCategory != DrugDesireCategory.Withdrawal)
				{
					result = 0;
				}
				else
				{
					result = 1;
				}
				return result;
			}
		}

		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}
	}
}
