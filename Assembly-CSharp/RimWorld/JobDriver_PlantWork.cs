using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_PlantWork : JobDriver
	{
		protected const TargetIndex PlantInd = TargetIndex.A;

		private float workDone;

		protected float xpPerTick;

		protected Plant Plant
		{
			get
			{
				return (Plant)base.CurJob.targetA.Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantWork.<MakeNewToils>c__Iterator46 <MakeNewToils>c__Iterator = new JobDriver_PlantWork.<MakeNewToils>c__Iterator46();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_PlantWork.<MakeNewToils>c__Iterator46 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		protected virtual void Init()
		{
		}
	}
}
