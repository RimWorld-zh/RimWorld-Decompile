using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Wear : JobDriver
	{
		private const int DurationTicks = 60;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			Toil gotoApparel = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0059: stateMachine*/)._003C_003Ef__this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0059: stateMachine*/)._003C_003Ef__this.TargetThingA, PathEndMode.ClosestTouch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoApparel.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoApparel;
			Toil prepare = new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 60
			};
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return prepare;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Apparel apparel = (Apparel)((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing;
					((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this.pawn.apparel.Wear(apparel, true);
					if (((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this.pawn.outfits != null && ((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this.CurJob.playerForced)
					{
						((_003CMakeNewToils_003Ec__Iterator54)/*Error near IL_0108: stateMachine*/)._003C_003Ef__this.pawn.outfits.forcedHandler.SetForced(apparel, true);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
