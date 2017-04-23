using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RearmTrap : JobDriver
	{
		private const int RearmTicks = 1125;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RearmTrap.<MakeNewToils>c__Iterator38 <MakeNewToils>c__Iterator = new JobDriver_RearmTrap.<MakeNewToils>c__Iterator38();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_RearmTrap.<MakeNewToils>c__Iterator38 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
