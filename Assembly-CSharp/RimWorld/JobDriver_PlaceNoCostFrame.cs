using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlaceNoCostFrame : JobDriver
	{
		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlaceNoCostFrame.<MakeNewToils>c__Iterator11 <MakeNewToils>c__Iterator = new JobDriver_PlaceNoCostFrame.<MakeNewToils>c__Iterator11();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_PlaceNoCostFrame.<MakeNewToils>c__Iterator11 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
