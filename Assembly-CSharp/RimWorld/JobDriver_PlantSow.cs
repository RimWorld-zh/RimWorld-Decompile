using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_PlantSow : JobDriver
	{
		private float sowWorkDone;

		private Plant Plant
		{
			get
			{
				return (Plant)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantSow.<MakeNewToils>c__Iterator45 <MakeNewToils>c__Iterator = new JobDriver_PlantSow.<MakeNewToils>c__Iterator45();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_PlantSow.<MakeNewToils>c__Iterator45 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
