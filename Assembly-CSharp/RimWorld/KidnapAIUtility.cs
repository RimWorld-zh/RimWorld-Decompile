using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D4 RID: 212
	public static class KidnapAIUtility
	{
		// Token: 0x060004BB RID: 1211 RVA: 0x0003544C File Offset: 0x0003384C
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
				Predicate<Thing> validator = delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					return pawn.RaceProps.Humanlike && pawn.Downed && pawn.Faction == Faction.OfPlayer && pawn.Faction.HostileTo(kidnapper.Faction) && kidnapper.CanReserve(pawn, 1, -1, null, false) && (disallowed == null || !disallowed.Contains(pawn));
				};
				victim = (Pawn)GenClosest.ClosestThingReachable(kidnapper.Position, kidnapper.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Some, false), maxDist, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				result = (victim != null);
			}
			return result;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00035524 File Offset: 0x00033924
		public static Pawn ReachableWoundedGuest(Pawn searcher)
		{
			List<Pawn> list = searcher.Map.mapPawns.SpawnedPawnsInFaction(searcher.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				if (pawn.guest != null && !pawn.IsPrisoner && pawn.Downed && searcher.CanReserveAndReach(pawn, PathEndMode.OnCell, Danger.Some, 1, -1, null, false))
				{
					return pawn;
				}
			}
			return null;
		}
	}
}
