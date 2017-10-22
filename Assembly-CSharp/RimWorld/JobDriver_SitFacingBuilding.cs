using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, base.job.def.joyMaxParticipants, 0, null) && base.pawn.Reserve(base.job.targetB, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		protected virtual void ModifyPlayToil(Toil toil)
		{
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
