using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_EnterTransporter : JobDriver
	{
		private TargetIndex TransporterInd = TargetIndex.A;

		private CompTransporter Transporter
		{
			get
			{
				Thing thing = base.CurJob.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator29)/*Error near IL_0046: stateMachine*/)._003C_003Ef__this.Transporter.LoadingInProgressOrReadyToLaunch));
			yield return Toils_Reserve.Reserve(this.TransporterInd, 1, -1, null);
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					CompTransporter transporter = ((_003CMakeNewToils_003Ec__Iterator29)/*Error near IL_00b1: stateMachine*/)._003C_003Ef__this.Transporter;
					((_003CMakeNewToils_003Ec__Iterator29)/*Error near IL_00b1: stateMachine*/)._003C_003Ef__this.pawn.DeSpawn();
					transporter.GetDirectlyHeldThings().TryAdd(((_003CMakeNewToils_003Ec__Iterator29)/*Error near IL_00b1: stateMachine*/)._003C_003Ef__this.pawn, true);
				}
			};
		}
	}
}
