using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Open : JobDriver
	{
		private const int OpenTicks = 300;

		private IOpenable Openable
		{
			get
			{
				return (IOpenable)base.CurJob.targetA.Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Open.<MakeNewToils>c__Iterator36 <MakeNewToils>c__Iterator = new JobDriver_Open.<MakeNewToils>c__Iterator36();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Open.<MakeNewToils>c__Iterator36 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
