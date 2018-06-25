using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E3 RID: 227
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x00036E00 File Offset: 0x00035200
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				IntVec3 intVec;
				return !t.IsForbidden(pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false) && pawn.carryTracker.MaxStackSpaceEver(t.def) > 0 && StoreUtility.TryFindBestBetterStoreCellFor(t, pawn, pawn.Map, StoreUtility.CurrentStoragePriorityOf(t), pawn.Faction, out intVec, true);
			};
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
			Job result;
			if (thing != null)
			{
				result = HaulAIUtility.HaulToStorageJob(pawn, thing);
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
