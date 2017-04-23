using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ConstructFinishFrame : JobDriver
	{
		private const int JobEndInterval = 5000;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ConstructFinishFrame.<MakeNewToils>c__IteratorE <MakeNewToils>c__IteratorE = new JobDriver_ConstructFinishFrame.<MakeNewToils>c__IteratorE();
			<MakeNewToils>c__IteratorE.<>f__this = this;
			JobDriver_ConstructFinishFrame.<MakeNewToils>c__IteratorE expr_0E = <MakeNewToils>c__IteratorE;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
