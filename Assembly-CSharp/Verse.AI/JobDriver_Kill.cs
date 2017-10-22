using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_Kill : JobDriver
	{
		private const TargetIndex VictimInd = TargetIndex.A;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.GetTarget(TargetIndex.A), base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Succeeded);
			yield return Toils_Combat.TrySetJobToUseAttackVerb();
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
