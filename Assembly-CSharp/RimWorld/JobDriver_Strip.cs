using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Strip : JobDriver
	{
		private const int StripTicks = 60;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Strip.<MakeNewToils>c__Iterator3C <MakeNewToils>c__Iterator3C = new JobDriver_Strip.<MakeNewToils>c__Iterator3C();
			<MakeNewToils>c__Iterator3C.<>f__this = this;
			JobDriver_Strip.<MakeNewToils>c__Iterator3C expr_0E = <MakeNewToils>c__Iterator3C;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
