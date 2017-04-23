using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		private const TargetIndex BuildingInd = TargetIndex.A;

		private const TargetIndex ComponentInd = TargetIndex.B;

		private const int TicksDuration = 1000;

		private Building Building
		{
			get
			{
				return (Building)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Thing Components
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FixBrokenDownBuilding.<MakeNewToils>c__Iterator10 <MakeNewToils>c__Iterator = new JobDriver_FixBrokenDownBuilding.<MakeNewToils>c__Iterator10();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_FixBrokenDownBuilding.<MakeNewToils>c__Iterator10 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
