using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006CD RID: 1741
	public class Medicine : ThingWithComps
	{
		// Token: 0x060025AA RID: 9642 RVA: 0x00142648 File Offset: 0x00140A48
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

		// Token: 0x04001502 RID: 5378
		private static List<Hediff> tendableHediffsInTendPriorityOrder = new List<Hediff>();

		// Token: 0x04001503 RID: 5379
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
