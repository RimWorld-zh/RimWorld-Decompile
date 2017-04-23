using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Vomit : JobDriver
	{
		private int ticksLeft;

		private PawnPosture lastPosture;

		public override PawnPosture Posture
		{
			get
			{
				return this.lastPosture;
			}
		}

		public override void Notify_LastPosture(PawnPosture posture, LayingDownState layingDown)
		{
			this.lastPosture = posture;
			this.layingDown = layingDown;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<PawnPosture>(ref this.lastPosture, "lastPosture", PawnPosture.Standing, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Vomit.<MakeNewToils>c__Iterator44 <MakeNewToils>c__Iterator = new JobDriver_Vomit.<MakeNewToils>c__Iterator44();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Vomit.<MakeNewToils>c__Iterator44 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
