using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_GoForWalk : JobDriver
	{
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			this.FailOn(() => !JoyUtility.EnjoyableOutsideNow(_003CMakeNewToils_003Ec__Iterator._0024this.pawn, null));
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickAction = delegate
			{
				if (Find.TickManager.TicksGame > _003CMakeNewToils_003Ec__Iterator._0024this.startTick + _003CMakeNewToils_003Ec__Iterator._0024this.job.def.joyDuration)
				{
					_003CMakeNewToils_003Ec__Iterator._0024this.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					JoyUtility.JoyTickCheckEnd(_003CMakeNewToils_003Ec__Iterator._0024this.pawn, JoyTickFullJoyAction.EndJob, 1f);
				}
			};
			yield return goToil;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
