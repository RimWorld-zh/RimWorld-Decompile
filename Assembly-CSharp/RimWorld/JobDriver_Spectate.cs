using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Spectate : JobDriver
	{
		private const TargetIndex MySpotOrChairInd = TargetIndex.A;

		private const TargetIndex WatchTargetInd = TargetIndex.B;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Spectate.<MakeNewToils>c__Iterator17 <MakeNewToils>c__Iterator = new JobDriver_Spectate.<MakeNewToils>c__Iterator17();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Spectate.<MakeNewToils>c__Iterator17 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
