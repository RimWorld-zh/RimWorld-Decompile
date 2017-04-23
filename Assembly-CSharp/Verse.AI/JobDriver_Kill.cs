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
			JobDriver_Kill.<MakeNewToils>c__Iterator1AF <MakeNewToils>c__Iterator1AF = new JobDriver_Kill.<MakeNewToils>c__Iterator1AF();
			<MakeNewToils>c__Iterator1AF.<>f__this = this;
			JobDriver_Kill.<MakeNewToils>c__Iterator1AF expr_0E = <MakeNewToils>c__Iterator1AF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
