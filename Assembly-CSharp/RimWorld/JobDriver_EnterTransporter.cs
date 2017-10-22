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
				Thing thing = base.job.GetTarget(this.TransporterInd).Thing;
				return (thing != null) ? thing.TryGetComp<CompTransporter>() : null;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.GetTarget(this.TransporterInd), base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0043: stateMachine*/)._0024this.Transporter.LoadingInProgressOrReadyToLaunch));
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
