using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				if (t.IsForbidden(pawn))
				{
					return false;
				}
				if (!HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false))
				{
					return false;
				}
				if (pawn.carryTracker.MaxStackSpaceEver(t.def) <= 0)
				{
					return false;
				}
				IntVec3 intVec = default(IntVec3);
				if (!StoreUtility.TryFindBestBetterStoreCellFor(t, pawn, pawn.Map, HaulAIUtility.StoragePriorityAtFor(t.Position, t), pawn.Faction, out intVec, true))
				{
					return false;
				}
				return true;
			};
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
			if (thing != null)
			{
				return HaulAIUtility.HaulToStorageJob(pawn, thing);
			}
			return null;
		}
	}
}
