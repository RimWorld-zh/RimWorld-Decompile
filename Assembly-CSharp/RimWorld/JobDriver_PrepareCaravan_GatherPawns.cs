using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;

		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PrepareCaravan_GatherPawns.<MakeNewToils>c__IteratorA <MakeNewToils>c__IteratorA = new JobDriver_PrepareCaravan_GatherPawns.<MakeNewToils>c__IteratorA();
			<MakeNewToils>c__IteratorA.<>f__this = this;
			JobDriver_PrepareCaravan_GatherPawns.<MakeNewToils>c__IteratorA expr_0E = <MakeNewToils>c__IteratorA;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private Toil SetFollowerToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					GatherAnimalsAndSlavesForCaravanUtility.SetFollower(this.AnimalOrSlave, this.pawn);
					RestUtility.WakeUp(this.pawn);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
