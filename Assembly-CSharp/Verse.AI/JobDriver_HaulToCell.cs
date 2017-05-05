using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public class JobDriver_HaulToCell : JobDriver
	{
		private const TargetIndex HaulableInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		public override string GetReport()
		{
			IntVec3 cell = this.pawn.jobs.curJob.targetB.Cell;
			Thing thing;
			if (this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			string text = null;
			SlotGroup slotGroup = cell.GetSlotGroup(base.Map);
			if (slotGroup != null)
			{
				text = slotGroup.parent.SlotYielderLabel();
			}
			string result;
			if (text != null)
			{
				result = "ReportHaulingTo".Translate(new object[]
				{
					thing.LabelCap,
					text
				});
			}
			else
			{
				result = "ReportHauling".Translate(new object[]
				{
					thing.LabelCap
				});
			}
			return result;
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HaulToCell.<MakeNewToils>c__Iterator1B6 <MakeNewToils>c__Iterator1B = new JobDriver_HaulToCell.<MakeNewToils>c__Iterator1B6();
			<MakeNewToils>c__Iterator1B.<>f__this = this;
			JobDriver_HaulToCell.<MakeNewToils>c__Iterator1B6 expr_0E = <MakeNewToils>c__Iterator1B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
