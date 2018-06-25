using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D3 RID: 1491
	public static class BestCaravanPawnUtility
	{
		// Token: 0x06001D0E RID: 7438 RVA: 0x000F8660 File Offset: 0x000F6A60
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.DiplomacyPower);
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000F8680 File Offset: 0x000F6A80
		public static Pawn FindBestNegotiator(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement);
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000F86A0 File Offset: 0x000F6AA0
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

		// Token: 0x06001D11 RID: 7441 RVA: 0x000F877C File Offset: 0x000F6B7C
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

		// Token: 0x06001D12 RID: 7442 RVA: 0x000F8810 File Offset: 0x000F6C10
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
