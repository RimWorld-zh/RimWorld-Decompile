using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Ignite : JobDriver
	{
		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Ignite.<MakeNewToils>c__Iterator2F <MakeNewToils>c__Iterator2F = new JobDriver_Ignite.<MakeNewToils>c__Iterator2F();
			<MakeNewToils>c__Iterator2F.<>f__this = this;
			JobDriver_Ignite.<MakeNewToils>c__Iterator2F expr_0E = <MakeNewToils>c__Iterator2F;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
