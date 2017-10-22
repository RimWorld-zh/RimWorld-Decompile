using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 intVec = default(IntVec3);
			Predicate<Thing> validator = (Predicate<Thing>)((Thing t) => (byte)((!t.IsForbidden(pawn)) ? (HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false) ? ((pawn.carryTracker.MaxStackSpaceEver(t.def) > 0) ? (StoreUtility.TryFindBestBetterStoreCellFor(t, pawn, pawn.Map, HaulAIUtility.StoragePriorityAtFor(t.Position, t), pawn.Faction, out intVec, true) ? 1 : 0) : 0) : 0) : 0) != 0);
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
			return (thing == null) ? null : HaulAIUtility.HaulToStorageJob(pawn, thing);
		}
	}
}
