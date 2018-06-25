using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A59 RID: 2649
	public static class JailbreakerMentalStateUtility
	{
		// Token: 0x04002545 RID: 9541
		private static List<Pawn> tmpPrisoners = new List<Pawn>();

		// Token: 0x06003AF7 RID: 15095 RVA: 0x001F4C10 File Offset: 0x001F3010
		public static Pawn FindPrisoner(Pawn pawn)
		{
			Pawn result;
			if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				JailbreakerMentalStateUtility.tmpPrisoners.Clear();
				List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction && pawn2 != pawn && !pawn2.Downed && !pawn2.InMentalState && !pawn2.IsBurning() && pawn2.Awake() && pawn2.guest.PrisonerIsSecure && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn2) && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						JailbreakerMentalStateUtility.tmpPrisoners.Add(pawn2);
					}
				}
				if (!JailbreakerMentalStateUtility.tmpPrisoners.Any<Pawn>())
				{
					result = null;
				}
				else
				{
					Pawn pawn3 = JailbreakerMentalStateUtility.tmpPrisoners.RandomElement<Pawn>();
					JailbreakerMentalStateUtility.tmpPrisoners.Clear();
					result = pawn3;
				}
			}
			return result;
		}
	}
}
