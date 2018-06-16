using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F8 RID: 1528
	public static class CaravanNameGenerator
	{
		// Token: 0x06001E65 RID: 7781 RVA: 0x00106ED4 File Offset: 0x001052D4
		public static string GenerateCaravanName(Caravan caravan)
		{
			Pawn pawn;
			if ((pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan)) == null)
			{
				pawn = (BestCaravanPawnUtility.FindBestDiplomat(caravan) ?? caravan.PawnsListForReading.Find((Pawn x) => caravan.IsOwner(x)));
			}
			Pawn pawn2 = pawn;
			string text = (pawn2 == null) ? caravan.def.label : "CaravanLeaderCaravanName".Translate(new object[]
			{
				pawn2.LabelShort
			}).CapitalizeFirst();
			for (int i = 1; i <= 1000; i++)
			{
				string text2 = text;
				if (i != 1)
				{
					text2 = text2 + " " + i;
				}
				if (!CaravanNameGenerator.CaravanNameInUse(text2))
				{
					return text2;
				}
			}
			Log.Error("Ran out of caravan names.", false);
			return caravan.def.label;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x00106FDC File Offset: 0x001053DC
		private static bool CaravanNameInUse(string name)
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}
	}
}
