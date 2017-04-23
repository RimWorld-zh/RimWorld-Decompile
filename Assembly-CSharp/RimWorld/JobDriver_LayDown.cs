using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_LayDown : JobDriver
	{
		private const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

		public Building_Bed Bed
		{
			get
			{
				return (Building_Bed)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_LayDown.<MakeNewToils>c__Iterator51 <MakeNewToils>c__Iterator = new JobDriver_LayDown.<MakeNewToils>c__Iterator51();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_LayDown.<MakeNewToils>c__Iterator51 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override string GetReport()
		{
			if (this.asleep)
			{
				return "ReportSleeping".Translate();
			}
			return "ReportResting".Translate();
		}
	}
}
