using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_RemoveBuilding : JobDriver
	{
		private float workLeft;

		private float totalNeededWork;

		protected Thing Target
		{
			get
			{
				return base.CurJob.targetA.Thing;
			}
		}

		protected Building Building
		{
			get
			{
				return (Building)this.Target.GetInnerIfMinified();
			}
		}

		protected abstract DesignationDef Designation
		{
			get;
		}

		protected abstract int TotalNeededWork
		{
			get;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Values.Look<float>(ref this.totalNeededWork, "totalNeededWork", 0f, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveBuilding.<MakeNewToils>c__IteratorF <MakeNewToils>c__IteratorF = new JobDriver_RemoveBuilding.<MakeNewToils>c__IteratorF();
			<MakeNewToils>c__IteratorF.<>f__this = this;
			JobDriver_RemoveBuilding.<MakeNewToils>c__IteratorF expr_0E = <MakeNewToils>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		protected virtual void FinishedRemoving()
		{
		}

		protected virtual void TickAction()
		{
		}
	}
}
