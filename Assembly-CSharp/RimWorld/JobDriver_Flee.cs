using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Flee : JobDriver
	{
		protected const TargetIndex DestInd = TargetIndex.A;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Flee.<MakeNewToils>c__Iterator2B <MakeNewToils>c__Iterator2B = new JobDriver_Flee.<MakeNewToils>c__Iterator2B();
			<MakeNewToils>c__Iterator2B.<>f__this = this;
			JobDriver_Flee.<MakeNewToils>c__Iterator2B expr_0E = <MakeNewToils>c__Iterator2B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
