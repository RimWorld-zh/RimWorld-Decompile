using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public class JobDriver_Kill : JobDriver
	{
		private const TargetIndex VictimInd = TargetIndex.A;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Kill.<MakeNewToils>c__Iterator1B0 <MakeNewToils>c__Iterator1B = new JobDriver_Kill.<MakeNewToils>c__Iterator1B0();
			<MakeNewToils>c__Iterator1B.<>f__this = this;
			JobDriver_Kill.<MakeNewToils>c__Iterator1B0 expr_0E = <MakeNewToils>c__Iterator1B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
