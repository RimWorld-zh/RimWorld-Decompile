using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000460 RID: 1120
	public class Hediff_Addiction : HediffWithComps
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x000AB4D0 File Offset: 0x000A98D0
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

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x000AB558 File Offset: 0x000A9958
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

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x000AB5B4 File Offset: 0x000A99B4
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

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x000AB614 File Offset: 0x000A9A14
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

		// Token: 0x060013B3 RID: 5043 RVA: 0x000AB649 File Offset: 0x000A9A49
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x04000BF3 RID: 3059
		private const int DefaultStageIndex = 0;

		// Token: 0x04000BF4 RID: 3060
		private const int WithdrawalStageIndex = 1;
	}
}
