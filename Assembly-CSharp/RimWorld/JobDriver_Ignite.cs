using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Ignite : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnBurningImmobile(TargetIndex.A);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator2F)/*Error near IL_0055: stateMachine*/)._003C_003Ef__this.pawn.natives.TryStartIgnite(((_003CMakeNewToils_003Ec__Iterator2F)/*Error near IL_0055: stateMachine*/)._003C_003Ef__this.TargetThingA);
				}
			};
		}
	}
}
