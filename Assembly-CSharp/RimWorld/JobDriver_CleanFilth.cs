using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_CleanFilth : JobDriver
	{
		private const TargetIndex FilthInd = TargetIndex.A;

		private float cleaningWorkDone;

		private float totalCleaningWorkDone;

		private float totalCleaningWorkRequired;

		private Filth Filth
		{
			get
			{
				return (Filth)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CleanFilth.<MakeNewToils>c__Iterator26 <MakeNewToils>c__Iterator = new JobDriver_CleanFilth.<MakeNewToils>c__Iterator26();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_CleanFilth.<MakeNewToils>c__Iterator26 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.cleaningWorkDone, "cleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkDone, "totalCleaningWorkDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f, false);
		}
	}
}
