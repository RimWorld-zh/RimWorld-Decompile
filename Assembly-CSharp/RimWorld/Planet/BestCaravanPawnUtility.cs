using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D5 RID: 1493
	public static class BestCaravanPawnUtility
	{
		// Token: 0x06001D13 RID: 7443 RVA: 0x000F84BC File Offset: 0x000F68BC
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.DiplomacyPower);
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000F84DC File Offset: 0x000F68DC
		public static Pawn FindBestNegotiator(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000F84FC File Offset: 0x000F68FC
		public static Pawn FindBestEntertainingPawnFor(Caravan caravan, Pawn forPawn)
		{
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn2 = caravan.pawns[i];
				if (pawn2 != forPawn && pawn2.RaceProps.Humanlike && !pawn2.Dead && !pawn2.Downed && !pawn2.InMentalState)
				{
					if (pawn2.IsPrisoner == forPawn.IsPrisoner)
					{
						if (!StatDefOf.SocialImpact.Worker.IsDisabledFor(pawn2))
						{
							float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true);
							if (pawn == null || statValue > num)
							{
								pawn = pawn2;
								num = statValue;
							}
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000F85D8 File Offset: 0x000F69D8
		public static Pawn FindPawnWithBestStat(Caravan caravan, StatDef stat)
		{
			Pawn pawn = null;
			float num = -1f;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (BestCaravanPawnUtility.IsConsciousOwner(pawn2, caravan))
				{
					if (!stat.Worker.IsDisabledFor(pawn2))
					{
						float statValue = pawn2.GetStatValue(stat, true);
						if (pawn == null || statValue > num)
						{
							pawn = pawn2;
							num = statValue;
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000F866C File Offset: 0x000F6A6C
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
