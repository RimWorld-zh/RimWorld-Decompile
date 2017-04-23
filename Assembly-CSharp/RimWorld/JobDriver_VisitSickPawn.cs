using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_VisitSickPawn : JobDriver
	{
		private const TargetIndex PatientInd = TargetIndex.A;

		private const TargetIndex ChairInd = TargetIndex.B;

		private Pawn Patient
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Thing Chair
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_VisitSickPawn.<MakeNewToils>c__Iterator23 <MakeNewToils>c__Iterator = new JobDriver_VisitSickPawn.<MakeNewToils>c__Iterator23();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_VisitSickPawn.<MakeNewToils>c__Iterator23 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
