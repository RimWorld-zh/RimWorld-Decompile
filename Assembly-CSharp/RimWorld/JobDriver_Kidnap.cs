using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Kidnap : JobDriver_TakeAndExitMap
	{
		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.Item;
			}
		}

		public override string GetReport()
		{
			if (this.pawn.HostileTo(this.Takee))
			{
				return base.GetReport();
			}
			return JobDefOf.Rescue.reportString.Replace("TargetA", this.Takee.LabelShort);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Kidnap.<MakeNewToils>c__Iterator31 <MakeNewToils>c__Iterator = new JobDriver_Kidnap.<MakeNewToils>c__Iterator31();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_Kidnap.<MakeNewToils>c__Iterator31 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
