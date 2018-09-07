using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public static class GiveToPackAnimalUtility
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public static IEnumerable<Pawn> CarrierCandidatesFor(Pawn pawn)
		{
			IEnumerable<Pawn> enumerable = (!pawn.IsFormingCaravan()) ? pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction) : pawn.GetLord().ownedPawns;
			enumerable = from x in enumerable
			where x.RaceProps.packAnimal && !x.inventory.UnloadEverything
			select x;
			if (pawn.Map.IsPlayerHome)
			{
				IEnumerable<Pawn> source = enumerable;
				if (GiveToPackAnimalUtility.<>f__mg$cache0 == null)
				{
					GiveToPackAnimalUtility.<>f__mg$cache0 = new Func<Pawn, bool>(CaravanFormingUtility.IsFormingCaravan);
				}
				enumerable = source.Where(GiveToPackAnimalUtility.<>f__mg$cache0);
			}
			return enumerable;
		}

		public static Pawn UsablePackAnimalWithTheMostFreeSpace(Pawn pawn)
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(pawn);
			Pawn pawn2 = null;
			float num = 0f;
			foreach (Pawn pawn3 in enumerable)
			{
				if (pawn3.RaceProps.packAnimal && pawn3 != pawn && pawn.CanReach(pawn3, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					float num2 = MassUtility.FreeSpace(pawn3);
					if (pawn2 == null || num2 > num)
					{
						pawn2 = pawn3;
						num = num2;
					}
				}
			}
			return pawn2;
		}

		[CompilerGenerated]
		private static bool <CarrierCandidatesFor>m__0(Pawn x)
		{
			return x.RaceProps.packAnimal && !x.inventory.UnloadEverything;
		}
	}
}
