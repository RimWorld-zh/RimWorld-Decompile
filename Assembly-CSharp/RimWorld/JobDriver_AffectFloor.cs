using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		private float workLeft = -1000f;

		protected bool clearSnow;

		protected abstract int BaseWorkAmount
		{
			get;
		}

		protected abstract DesignationDef DesDef
		{
			get;
		}

		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AffectFloor.<MakeNewToils>c__IteratorB <MakeNewToils>c__IteratorB = new JobDriver_AffectFloor.<MakeNewToils>c__IteratorB();
			<MakeNewToils>c__IteratorB.<>f__this = this;
			JobDriver_AffectFloor.<MakeNewToils>c__IteratorB expr_0E = <MakeNewToils>c__IteratorB;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		protected abstract void DoEffect(IntVec3 c);

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}
	}
}
