using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[CompilerGenerated]
		private sealed class <TryFindGoodKidnapVictim>c__AnonStorey0
		{
			internal Pawn kidnapper;

			internal List<Thing> disallowed;

			public <TryFindGoodKidnapVictim>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				Pawn pawn = t as Pawn;
				return pawn.RaceProps.Humanlike && pawn.Downed && pawn.Faction == Faction.OfPlayer && pawn.Faction.HostileTo(this.kidnapper.Faction) && this.kidnapper.CanReserve(pawn, 1, -1, null, false) && (this.disallowed == null || !this.disallowed.Contains(pawn));
			}
		}
	}
}
