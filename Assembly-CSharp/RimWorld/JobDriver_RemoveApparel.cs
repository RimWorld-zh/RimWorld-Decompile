using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RemoveApparel : JobDriver
	{
		private const int DurationTicks = 60;

		private Apparel TargetApparel
		{
			get
			{
				return (Apparel)base.TargetA.Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveApparel.<MakeNewToils>c__Iterator53 <MakeNewToils>c__Iterator = new JobDriver_RemoveApparel.<MakeNewToils>c__Iterator53();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_RemoveApparel.<MakeNewToils>c__Iterator53 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
