using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000464 RID: 1124
	public class Hediff_Addiction : HediffWithComps
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x000AB4C0 File Offset: 0x000A98C0
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
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x000AB548 File Offset: 0x000A9948
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
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x000AB5A4 File Offset: 0x000A99A4
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
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x000AB604 File Offset: 0x000A9A04
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

		// Token: 0x060013BC RID: 5052 RVA: 0x000AB639 File Offset: 0x000A9A39
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x04000BF6 RID: 3062
		private const int DefaultStageIndex = 0;

		// Token: 0x04000BF7 RID: 3063
		private const int WithdrawalStageIndex = 1;
	}
}
