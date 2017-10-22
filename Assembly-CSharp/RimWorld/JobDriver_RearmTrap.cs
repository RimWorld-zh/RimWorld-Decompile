using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RearmTrap : JobDriver
	{
		private const int RearmTicks = 1125;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.RearmTrap);
			Toil gotoThing = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_005b: stateMachine*/)._0024this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_005b: stateMachine*/)._0024this.TargetThingA, PathEndMode.Touch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
