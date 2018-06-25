using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CB RID: 1739
	public class Medicine : ThingWithComps
	{
		// Token: 0x04001504 RID: 5380
		private static List<Hediff> tendableHediffsInTendPriorityOrder = new List<Hediff>();

		// Token: 0x04001505 RID: 5381
		private static List<Hediff> tmpHediffs = new List<Hediff>();

		// Token: 0x060025A7 RID: 9639 RVA: 0x00142BBC File Offset: 0x00140FBC
		public static int GetMedicineCountToFullyHeal(Pawn pawn)
		{
			int num = 0;
			int num2 = pawn.health.hediffSet.hediffs.Count + 1;
			Medicine.tendableHediffsInTendPriorityOrder.Clear();
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].TendableNow(false))
				{
					Medicine.tendableHediffsInTendPriorityOrder.Add(hediffs[i]);
				}
			}
			TendUtility.SortByTendPriority(Medicine.tendableHediffsInTendPriorityOrder);
			int num3 = 0;
			for (;;)
			{
				num++;
				if (num > num2)
				{
					break;
				}
				TendUtility.GetOptimalHediffsToTendWithSingleTreatment(pawn, true, Medicine.tmpHediffs, Medicine.tendableHediffsInTendPriorityOrder);
				if (!Medicine.tmpHediffs.Any<Hediff>())
				{
					goto IL_102;
				}
				num3++;
				for (int j = 0; j < Medicine.tmpHediffs.Count; j++)
				{
					Medicine.tendableHediffsInTendPriorityOrder.Remove(Medicine.tmpHediffs[j]);
				}
			}
			Log.Error("Too many iterations.", false);
			IL_102:
			Medicine.tmpHediffs.Clear();
			Medicine.tendableHediffsInTendPriorityOrder.Clear();
			return num3;
		}
	}
}
