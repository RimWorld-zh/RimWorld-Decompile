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
				return (Thing)base.CurJob.GetTarget(TargetIndex.B);
			}
		}

		public override string GetReport()
		{
			Thing thing = null;
			thing = ((base.pawn.carryTracker.CarriedThing == null) ? base.TargetThingA : base.pawn.carryTracker.CarriedThing);
			return "ReportHaulingTo".Translate(thing.LabelCap, base.CurJob.targetB.Thing.LabelShort);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn((Func<bool>)(() => TransporterUtility.WasLoadingCanceled(((_003CMakeNewToils_003Ec__Iterator1BB)/*Error near IL_0071: stateMachine*/)._003C_003Ef__this.Container)));
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.ReserveQueue(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Reserve.ReserveQueue(TargetIndex.B, 1, -1, null);
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return getToHaulTarget;
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true);
			yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
		}
	}
}
