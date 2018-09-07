using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		public JobGiver_Haul()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				IntVec3 intVec;
				return !t.IsForbidden(pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false) && pawn.carryTracker.MaxStackSpaceEver(t.def) > 0 && StoreUtility.TryFindBestBetterStoreCellFor(t, pawn, pawn.Map, StoreUtility.CurrentStoragePriorityOf(t), pawn.Faction, out intVec, true);
			};
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
			if (thing != null)
			{
				return HaulAIUtility.HaulToStorageJob(pawn, thing);
			}
			return null;
		}

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				IntVec3 intVec;
				return !t.IsForbidden(this.pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(this.pawn, t, false) && this.pawn.carryTracker.MaxStackSpaceEver(t.def) > 0 && StoreUtility.TryFindBestBetterStoreCellFor(t, this.pawn, this.pawn.Map, StoreUtility.CurrentStoragePriorityOf(t), this.pawn.Faction, out intVec, true);
			}
		}
	}
}
