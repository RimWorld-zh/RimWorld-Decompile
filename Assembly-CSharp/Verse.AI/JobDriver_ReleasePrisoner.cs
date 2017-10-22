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
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOn((Func<bool>)(() => ((Pawn)(Thing)((_003CMakeNewToils_003Ec__Iterator3A)/*Error near IL_0055: stateMachine*/)._003C_003Ef__this.GetActor().CurJob.GetTarget(TargetIndex.A)).guest.interactionMode != PrisonerInteractionModeDefOf.Release));
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			Toil reserveTargetA = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return reserveTargetA;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator3A)/*Error near IL_00b0: stateMachine*/)._003C_003Ef__this.Prisoner.IsPrisonerOfColony || !((_003CMakeNewToils_003Ec__Iterator3A)/*Error near IL_00b0: stateMachine*/)._003C_003Ef__this.Prisoner.guest.PrisonerIsSecure)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, false);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator3A)/*Error near IL_0146: stateMachine*/)._003CsetReleased_003E__2.actor;
					Job curJob = actor.jobs.curJob;
					Pawn p = curJob.targetA.Thing as Pawn;
					GenGuest.PrisonerRelease(p);
				}
			};
		}
	}
}
