using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_GoForWalk : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => !JoyUtility.EnjoyableOutsideNow(((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_002b: stateMachine*/)._003C_003Ef__this.pawn, null)));
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickAction = (Action)delegate
			{
				if (Find.TickManager.TicksGame > ((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.startTick + ((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.CurJob.def.joyDuration)
				{
					((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_0050: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.EndJob, 1f);
				}
			};
			yield return goToil;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.CurJob.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = ((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.CurJob.targetQueueA[0];
						((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.CurJob.targetQueueA.RemoveAt(0);
						((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.CurJob.targetA = targetA;
						((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_008a: stateMachine*/)._003C_003Ef__this.JumpToToil(((_003CMakeNewToils_003Ec__Iterator1B)/*Error near IL_008a: stateMachine*/)._003CgoToil_003E__0);
					}
				}
			};
		}
	}
}
