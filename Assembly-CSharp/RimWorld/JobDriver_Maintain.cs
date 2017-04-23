using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Maintain : JobDriver
	{
		private const int MaintainTicks = 180;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Maintain.<MakeNewToils>c__Iterator33 <MakeNewToils>c__Iterator = new JobDriver_Maintain.<MakeNewToils>c__Iterator33();
			JobDriver_Maintain.<MakeNewToils>c__Iterator33 expr_07 = <MakeNewToils>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
