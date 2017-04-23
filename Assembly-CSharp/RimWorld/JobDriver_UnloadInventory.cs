using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_UnloadInventory : JobDriver
	{
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		private const TargetIndex StoreCellInd = TargetIndex.C;

		private const int UnloadDuration = 10;

		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UnloadInventory.<MakeNewToils>c__Iterator40 <MakeNewToils>c__Iterator = new JobDriver_UnloadInventory.<MakeNewToils>c__Iterator40();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_UnloadInventory.<MakeNewToils>c__Iterator40 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
