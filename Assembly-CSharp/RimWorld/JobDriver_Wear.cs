using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Wear : JobDriver
	{
		private const int DurationTicks = 60;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Wear.<MakeNewToils>c__Iterator54 <MakeNewToils>c__Iterator = new JobDriver_Wear.<MakeNewToils>c__Iterator54();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Wear.<MakeNewToils>c__Iterator54 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
