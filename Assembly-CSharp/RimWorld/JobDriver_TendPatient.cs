using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TendPatient : JobDriver
	{
		private const int BaseTendDuration = 600;

		private bool usesMedicine;

		protected Thing MedicineUsed
		{
			get
			{
				return base.CurJob.targetB.Thing;
			}
		}

		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)base.CurJob.targetA.Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TendPatient.<MakeNewToils>c__Iterator19 <MakeNewToils>c__Iterator = new JobDriver_TendPatient.<MakeNewToils>c__Iterator19();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_TendPatient.<MakeNewToils>c__Iterator19 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
