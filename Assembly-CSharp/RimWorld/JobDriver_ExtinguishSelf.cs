using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		protected const int NumSpeechesToSay = 5;

		protected Fire TargetFire
		{
			get
			{
				return (Fire)base.CurJob.targetA.Thing;
			}
		}

		public override string GetReport()
		{
			if (this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(new object[]
				{
					this.TargetFire.parent.LabelCap
				});
			}
			return "ReportExtinguishingFire".Translate();
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtinguishSelf.<MakeNewToils>c__Iterator15 <MakeNewToils>c__Iterator = new JobDriver_ExtinguishSelf.<MakeNewToils>c__Iterator15();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_ExtinguishSelf.<MakeNewToils>c__Iterator15 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
