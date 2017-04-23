using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_AffectRoof : JobDriver
	{
		private const TargetIndex CellInd = TargetIndex.A;

		private const TargetIndex GotoTargetInd = TargetIndex.B;

		private const float BaseWorkAmount = 65f;

		private float workLeft;

		protected IntVec3 Cell
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Cell;
			}
		}

		protected abstract PathEndMode PathEndMode
		{
			get;
		}

		protected abstract void DoEffect();

		protected abstract bool DoWorkFailOn();

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AffectRoof.<MakeNewToils>c__IteratorC <MakeNewToils>c__IteratorC = new JobDriver_AffectRoof.<MakeNewToils>c__IteratorC();
			<MakeNewToils>c__IteratorC.<>f__this = this;
			JobDriver_AffectRoof.<MakeNewToils>c__IteratorC expr_0E = <MakeNewToils>c__IteratorC;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
