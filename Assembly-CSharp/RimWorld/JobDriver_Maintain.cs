using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Maintain : JobDriver
	{
		private const int MaintainTicks = 180;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil prepare = Toils_General.Wait(180);
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return prepare;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator33)/*Error near IL_00c8: stateMachine*/)._003Cmaintain_003E__1.actor;
					CompMaintainable compMaintainable = actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>();
					compMaintainable.Maintained();
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
