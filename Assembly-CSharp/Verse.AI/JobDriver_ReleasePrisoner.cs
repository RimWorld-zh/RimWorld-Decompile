using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		private const TargetIndex PrisonerInd = TargetIndex.A;

		private const TargetIndex ReleaseCellInd = TargetIndex.B;

		private Pawn Prisoner
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator3A <MakeNewToils>c__Iterator3A = new JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator3A();
			<MakeNewToils>c__Iterator3A.<>f__this = this;
			JobDriver_ReleasePrisoner.<MakeNewToils>c__Iterator3A expr_0E = <MakeNewToils>c__Iterator3A;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
