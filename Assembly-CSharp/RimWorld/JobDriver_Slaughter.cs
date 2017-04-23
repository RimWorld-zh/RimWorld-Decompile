using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Slaughter : JobDriver
	{
		private const int SlaughterDuration = 180;

		protected Pawn Victim
		{
			get
			{
				return (Pawn)base.CurJob.targetA.Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Slaughter.<MakeNewToils>c__Iterator7 <MakeNewToils>c__Iterator = new JobDriver_Slaughter.<MakeNewToils>c__Iterator7();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Slaughter.<MakeNewToils>c__Iterator7 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
