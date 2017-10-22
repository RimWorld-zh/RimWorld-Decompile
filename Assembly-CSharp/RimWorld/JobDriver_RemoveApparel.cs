using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RemoveApparel : JobDriver
	{
		private const int DurationTicks = 60;

		private Apparel TargetApparel
		{
			get
			{
				return (Apparel)base.TargetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0059: stateMachine*/)._0024this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 60
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
