using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		private const TargetIndex ItemInd = TargetIndex.A;

		private const TargetIndex ExitCellInd = TargetIndex.B;

		protected Thing Item
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeAndExitMap.<MakeNewToils>c__Iterator30 <MakeNewToils>c__Iterator = new JobDriver_TakeAndExitMap.<MakeNewToils>c__Iterator30();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_TakeAndExitMap.<MakeNewToils>c__Iterator30 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
