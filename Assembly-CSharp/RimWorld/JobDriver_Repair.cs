using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Repair : JobDriver
	{
		private const float WarmupTicks = 80f;

		private const float TicksBetweenRepairs = 20f;

		protected float ticksToNextRepair;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Repair.<MakeNewToils>c__Iterator13 <MakeNewToils>c__Iterator = new JobDriver_Repair.<MakeNewToils>c__Iterator13();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Repair.<MakeNewToils>c__Iterator13 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
