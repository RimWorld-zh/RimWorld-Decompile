using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A87 RID: 2695
	public static class SlaughtererMentalStateUtility
	{
		// Token: 0x04002585 RID: 9605
		private static List<Pawn> tmpAnimals = new List<Pawn>();

		// Token: 0x06003BD6 RID: 15318 RVA: 0x001F8B7C File Offset: 0x001F6F7C
		public static Pawn FindAnimal(Pawn pawn)
		{
			Pawn result;
			if (!pawn.Spawned)
			{
				result = null;
			}
			else
			{
				SlaughtererMentalStateUtility.tmpAnimals.Clear();
				List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2.RaceProps.Animal && pawn2.Faction == pawn.Faction && pawn2 != pawn && !pawn2.IsBurning() && !pawn2.InAggroMentalState && pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						SlaughtererMentalStateUtility.tmpAnimals.Add(pawn2);
					}
				}
				if (!SlaughtererMentalStateUtility.tmpAnimals.Any<Pawn>())
				{
					result = null;
				}
				else
				{
					Pawn pawn3 = SlaughtererMentalStateUtility.tmpAnimals.RandomElement<Pawn>();
					SlaughtererMentalStateUtility.tmpAnimals.Clear();
					result = pawn3;
				}
			}
			return result;
		}
	}
}
