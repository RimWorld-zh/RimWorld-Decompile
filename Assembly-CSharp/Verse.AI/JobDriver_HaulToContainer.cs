using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_HaulToContainer : JobDriver
	{
		private const TargetIndex CarryThingIndex = TargetIndex.A;

		private const TargetIndex DestIndex = TargetIndex.B;

		private const TargetIndex PrimaryDestIndex = TargetIndex.C;

		private Thing Container
		{
			get
			{
				return (Thing)base.job.GetTarget(TargetIndex.B);
			}
		}

		public override string GetReport()
		{
			Thing thing = null;
			thing = ((base.pawn.CurJob != base.job || base.pawn.carryTracker.CarriedThing == null) ? base.TargetThingA : base.pawn.carryTracker.CarriedThing);
			return (thing != null && base.job.targetB.HasThing) ? "ReportHaulingTo".Translate(thing.LabelCap, base.job.targetB.Thing.LabelShort) : "ReportHaulingUnknown".Translate();
		}

		public override bool TryMakePreToilReservations()
		{
			base.pawn.ReserveAsManyAsPossible(base.job.GetTargetQueue(TargetIndex.A), base.job, 1, -1, null);
			base.pawn.ReserveAsManyAsPossible(base.job.GetTargetQueue(TargetIndex.B), base.job, 1, -1, null);
			return base.pawn.Reserve(base.job.GetTarget(TargetIndex.A), base.job, 1, -1, null) && base.pawn.Reserve(base.job.GetTarget(TargetIndex.B), base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn((Func<bool>)(() => TransporterUtility.WasLoadingCanceled(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0062: stateMachine*/)._0024this.Container)));
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return getToHaulTarget;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
