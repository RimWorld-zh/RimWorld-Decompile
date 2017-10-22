using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Spectate : JobDriver
	{
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		private const TargetIndex WatchTargetInd = TargetIndex.B;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.GetTarget(TargetIndex.A), base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (base.job.GetTarget(TargetIndex.A).HasThing)
			{
				this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
