using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		protected const TargetIndex TargetThingIndex = TargetIndex.A;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_VisitJoyThing.<MakeNewToils>c__Iterator22 <MakeNewToils>c__Iterator = new JobDriver_VisitJoyThing.<MakeNewToils>c__Iterator22();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_VisitJoyThing.<MakeNewToils>c__Iterator22 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		protected abstract Action GetWaitTickAction();
	}
}
