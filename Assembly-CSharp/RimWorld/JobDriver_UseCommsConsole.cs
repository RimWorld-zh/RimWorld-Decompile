using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_UseCommsConsole : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn((Func<Toil, bool>)delegate(Toil to)
			{
				Building_CommsConsole building_CommsConsole2 = (Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				return !building_CommsConsole2.CanUseCommsNow;
			});
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator42)/*Error near IL_0090: stateMachine*/)._003CopenComms_003E__0.actor;
					Building_CommsConsole building_CommsConsole = (Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
					if (building_CommsConsole.CanUseCommsNow)
					{
						actor.jobs.curJob.commTarget.TryOpenComms(actor);
					}
				}
			};
		}
	}
}
