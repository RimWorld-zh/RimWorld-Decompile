using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Nuzzle : JobDriver
	{
		private const int NuzzleDuration = 100;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 100, false, true);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator2)/*Error near IL_00cc: stateMachine*/)._003Cfinalize_003E__0.actor;
					Pawn recipient = (Pawn)actor.CurJob.targetA.Thing;
					actor.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
