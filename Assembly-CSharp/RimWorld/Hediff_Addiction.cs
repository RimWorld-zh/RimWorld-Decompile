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
				Need_Chemical result;
				List<Need> allNeeds;
				int i;
				if (base.pawn.Dead)
				{
					result = null;
				}
				else
				{
					allNeeds = base.pawn.needs.AllNeeds;
					for (i = 0; i < allNeeds.Count; i++)
					{
						if (allNeeds[i].def == base.def.causesNeed)
							goto IL_004d;
					}
					result = null;
				}
				goto IL_0077;
				IL_004d:
				result = (Need_Chemical)allNeeds[i];
				goto IL_0077;
				IL_0077:
				return result;
			}
		}

		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				int num = 0;
				ChemicalDef result;
				while (true)
				{
					if (num < allDefsListForReading.Count)
					{
						if (allDefsListForReading[num].addictionHediff == base.def)
						{
							result = allDefsListForReading[num];
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
			}
		}

		public override string LabelInBrackets
		{
			get
			{
				return (this.CurStageIndex != 1 || base.def.CompProps<HediffCompProperties_SeverityPerDay>() == null) ? base.LabelInBrackets : (base.LabelInBrackets + " " + ((float)(1.0 - this.Severity)).ToStringPercent());
			}
		}

		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				return (need != null && need.CurCategory == DrugDesireCategory.Withdrawal) ? 1 : 0;
			}
		}

		public void Notify_NeedCategoryChanged()
		{
			base.pawn.health.Notify_HediffChanged(this);
		}
	}
}
