using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Nuzzle : JobDriver
	{
		private const int NuzzleDuration = 100;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Nuzzle.<MakeNewToils>c__Iterator2 <MakeNewToils>c__Iterator = new JobDriver_Nuzzle.<MakeNewToils>c__Iterator2();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Nuzzle.<MakeNewToils>c__Iterator2 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
