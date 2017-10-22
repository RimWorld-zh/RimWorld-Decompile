using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		private const TargetIndex ItemInd = TargetIndex.A;

		private const TargetIndex ExitCellInd = TargetIndex.B;

		protected Thing Item
		{
			get
			{
				return base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Item, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
