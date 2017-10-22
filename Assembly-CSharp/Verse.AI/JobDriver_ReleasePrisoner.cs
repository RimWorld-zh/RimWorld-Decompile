using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		private const TargetIndex PrisonerInd = TargetIndex.A;

		private const TargetIndex ReleaseCellInd = TargetIndex.B;

		private Pawn Prisoner
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Prisoner, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0042: stateMachine*/;
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOn((Func<bool>)(() => ((Pawn)(Thing)_003CMakeNewToils_003Ec__Iterator._0024this.GetActor().CurJob.GetTarget(TargetIndex.A)).guest.interactionMode != PrisonerInteractionModeDefOf.Release));
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn((Func<bool>)(() => !_003CMakeNewToils_003Ec__Iterator._0024this.Prisoner.IsPrisonerOfColony || !_003CMakeNewToils_003Ec__Iterator._0024this.Prisoner.guest.PrisonerIsSecure)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
