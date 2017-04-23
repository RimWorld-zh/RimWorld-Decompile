using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		private const int UnloadDuration = 10;

		private int countToDrop = -1;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UnloadYourInventory.<MakeNewToils>c__Iterator41 <MakeNewToils>c__Iterator = new JobDriver_UnloadYourInventory.<MakeNewToils>c__Iterator41();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_UnloadYourInventory.<MakeNewToils>c__Iterator41 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
