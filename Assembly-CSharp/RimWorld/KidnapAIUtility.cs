using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class KidnapAIUtility
	{
		public static bool TryFindGoodKidnapVictim(Pawn kidnapper, float maxDist, out Pawn victim, List<Thing> disallowed = null)
		{
			bool result;
			if (!kidnapper.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !kidnapper.Map.reachability.CanReachMapEdge(kidnapper.Position, TraverseParms.For(kidnapper, Danger.Some, TraverseMode.ByPawn, false)))
			{
				victim = null;
				result = false;
			}
			else
			{
				Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					return (byte)(pawn.RaceProps.Humanlike ? (pawn.Downed ? ((pawn.Faction == Faction.OfPlayer) ? (pawn.Faction.HostileTo(kidnapper.Faction) ? (kidnapper.CanReserve((Thing)pawn, 1, -1, null, false) ? ((disallowed == null || !disallowed.Contains(pawn)) ? 1 : 0) : 0) : 0) : 0) : 0) : 0) != 0;
				};
				victim = (Pawn)GenClosest.ClosestThingReachable(kidnapper.Position, kidnapper.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				result = (victim != null);
			}
			return result;
		}

		public static Pawn ReachableWoundedGuest(Pawn searcher)
		{
			List<Pawn> list = searcher.Map.mapPawns.SpawnedPawnsInFaction(searcher.Faction);
			int num = 0;
			Pawn result;
			while (true)
			{
				if (num < list.Count)
				{
					Pawn pawn = list[num];
					if (pawn.guest != null && !pawn.IsPrisoner && pawn.Downed && searcher.CanReserveAndReach((Thing)pawn, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
					{
						result = pawn;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
