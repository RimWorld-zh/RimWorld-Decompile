using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Strip : JobDriver
	{
		private const int StripTicks = 60;

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !StrippableUtility.CanBeStrippedByColony(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004a: stateMachine*/)._0024this.TargetThingA)));
			Toil gotoThing = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006e: stateMachine*/)._0024this.pawn.pather.StartPath(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_006e: stateMachine*/)._0024this.TargetThingA, PathEndMode.ClosestTouch);
				},
				defaultCompleteMode = ToilCompleteMode.PatherArrival
			};
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override object[] TaleParameters()
		{
			Corpse corpse = base.TargetA.Thing as Corpse;
			return new object[2]
			{
				base.pawn,
				(corpse == null) ? base.TargetA.Thing : corpse.InnerPawn
			};
		}
	}
}
