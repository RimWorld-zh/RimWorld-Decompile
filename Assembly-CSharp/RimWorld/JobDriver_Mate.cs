using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Mate : JobDriver
	{
		private const int MateDuration = 500;

		private const TargetIndex FemInd = TargetIndex.A;

		private const int TicksBetweenHeartMotes = 100;

		private Pawn Female
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Mate.<MakeNewToils>c__Iterator1 <MakeNewToils>c__Iterator = new JobDriver_Mate.<MakeNewToils>c__Iterator1();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Mate.<MakeNewToils>c__Iterator1 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
