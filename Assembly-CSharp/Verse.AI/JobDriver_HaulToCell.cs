using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_HaulToCell : JobDriver
	{
		private const TargetIndex HaulableInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		public override string GetReport()
		{
			IntVec3 cell = base.pawn.jobs.curJob.targetB.Cell;
			Thing thing = null;
			thing = ((base.pawn.carryTracker.CarriedThing == null) ? base.TargetThingA : base.pawn.carryTracker.CarriedThing);
			string text = (string)null;
			SlotGroup slotGroup = cell.GetSlotGroup(base.Map);
			if (slotGroup != null)
			{
				text = slotGroup.parent.SlotYielderLabel();
			}
			return (text == null) ? "ReportHauling".Translate(thing.LabelCap) : "ReportHaulingTo".Translate(thing.LabelCap, text);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			if (!base.TargetThingA.IsForbidden(base.pawn))
			{
				this.FailOnForbidden(TargetIndex.A);
			}
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			Toil reserveTargetA = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return reserveTargetA;
			Toil toilGoto = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn((Func<bool>)delegate
			{
				Pawn actor = ((_003CMakeNewToils_003Ec__Iterator1BA)/*Error near IL_00d7: stateMachine*/)._003CtoilGoto_003E__1.actor;
				Job curJob = actor.jobs.curJob;
				if (curJob.haulMode == HaulMode.ToCellStorage)
				{
					Thing thing = curJob.GetTarget(TargetIndex.A).Thing;
					IntVec3 cell = actor.jobs.curJob.GetTarget(TargetIndex.B).Cell;
					if (!cell.IsValidStorageFor(((_003CMakeNewToils_003Ec__Iterator1BA)/*Error near IL_00d7: stateMachine*/)._003C_003Ef__this.Map, thing))
					{
						return true;
					}
				}
				return false;
			});
			yield return toilGoto;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true);
			if (base.CurJob.haulOpportunisticDuplicates)
			{
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveTargetA, TargetIndex.A, TargetIndex.B, false, null);
			}
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
		}
	}
}
