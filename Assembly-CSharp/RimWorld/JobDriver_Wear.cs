using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Wear : JobDriver
	{
		private const int DurationTicks = 60;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil gotoApparel = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003c: stateMachine*/)._0024this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003c: stateMachine*/)._0024this.TargetThingA, PathEndMode.ClosestTouch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoApparel.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoApparel;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
