using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Flee : JobDriver
	{
		protected const TargetIndex DestInd = TargetIndex.A;

		protected const TargetIndex DangerInd = TargetIndex.B;

		public override bool TryMakePreToilReservations()
		{
			base.pawn.Map.pawnDestinationReservationManager.Reserve(base.pawn, base.job, base.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0050: stateMachine*/)._0024this.pawn.IsColonist)
					{
						MoteMaker.MakeColonistActionOverlay(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0050: stateMachine*/)._0024this.pawn, ThingDefOf.Mote_ColonistFleeing);
					}
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
