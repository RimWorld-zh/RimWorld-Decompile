using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		private const TargetIndex TakeeInd = TargetIndex.A;

		private const TargetIndex DropPodInd = TargetIndex.B;

		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryToCryptosleepCasket.<MakeNewToils>c__Iterator25 <MakeNewToils>c__Iterator = new JobDriver_CarryToCryptosleepCasket.<MakeNewToils>c__Iterator25();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_CarryToCryptosleepCasket.<MakeNewToils>c__Iterator25 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
