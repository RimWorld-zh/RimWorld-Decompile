using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_LayEgg : JobDriver
	{
		private const int LayEgg = 500;

		private const TargetIndex LaySpotInd = TargetIndex.A;

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 500
			};
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0092: stateMachine*/)._003Cfinalize_003E__1.actor;
					Thing forbiddenIfOutsideHomeArea = GenSpawn.Spawn(actor.GetComp<CompEggLayer>().ProduceEgg(), actor.Position, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0092: stateMachine*/)._003C_003Ef__this.Map);
					forbiddenIfOutsideHomeArea.SetForbiddenIfOutsideHomeArea();
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
