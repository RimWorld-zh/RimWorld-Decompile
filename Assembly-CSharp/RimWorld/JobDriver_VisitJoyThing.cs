using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		protected const TargetIndex TargetThingIndex = TargetIndex.A;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.GetTarget(TargetIndex.A), base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		protected abstract Action GetWaitTickAction();
	}
}
