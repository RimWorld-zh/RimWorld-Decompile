using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A8A RID: 2698
	public static class SlaughtererMentalStateUtility
	{
		// Token: 0x04002596 RID: 9622
		private static List<Pawn> tmpAnimals = new List<Pawn>();

		// Token: 0x06003BDB RID: 15323 RVA: 0x001F8FD4 File Offset: 0x001F73D4
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
