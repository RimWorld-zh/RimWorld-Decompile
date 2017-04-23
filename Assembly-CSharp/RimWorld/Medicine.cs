using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Medicine : ThingWithComps
	{
		private static HashSet<Hediff> tmpHandledHediffs = new HashSet<Hediff>();

		private static List<Hediff> tmpHediffs = new List<Hediff>();

		public static int GetMedicineCountToFullyHeal(Pawn pawn)
		{
			int num = 0;
			int num2 = pawn.health.hediffSet.hediffs.Count + 1;
			Medicine.tmpHandledHediffs.Clear();
			int num3 = 0;
			while (true)
			{
				num++;
				if (num > num2)
				{
					break;
				}
				TendUtility.GetOptimalHediffsToTendWithSingleTreatment(pawn, true, Medicine.tmpHediffs, Medicine.tmpHandledHediffs);
				if (!Medicine.tmpHediffs.Any<Hediff>())
				{
					goto IL_9A;
				}
				num3++;
				for (int i = 0; i < Medicine.tmpHediffs.Count; i++)
				{
					Medicine.tmpHandledHediffs.Add(Medicine.tmpHediffs[i]);
				}
			}
			Log.Error("Too many iterations.");
			IL_9A:
			Medicine.tmpHandledHediffs.Clear();
			return num3;
		}
	}
}
