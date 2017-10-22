using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlayBilliards : JobDriver
	{
		private const int ShotDuration = 600;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, base.job.def.joyMaxParticipants, 0, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			Toil chooseCell = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
			yield return chooseCell;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override object[] TaleParameters()
		{
			return new object[2]
			{
				base.pawn,
				base.TargetA.Thing.def
			};
		}
	}
}
