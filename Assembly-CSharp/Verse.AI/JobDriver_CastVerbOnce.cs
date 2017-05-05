using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public class JobDriver_CastVerbOnce : JobDriver
	{
		public override string GetReport()
		{
			string text;
			if (base.TargetA.HasThing)
			{
				text = base.TargetThingA.LabelCap;
			}
			else
			{
				text = "AreaLower".Translate();
			}
			return "UsingVerb".Translate(new object[]
			{
				base.CurJob.verbToUse.verbProps.label,
				text
			});
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CastVerbOnce.<MakeNewToils>c__Iterator1B4 <MakeNewToils>c__Iterator1B = new JobDriver_CastVerbOnce.<MakeNewToils>c__Iterator1B4();
			JobDriver_CastVerbOnce.<MakeNewToils>c__Iterator1B4 expr_07 = <MakeNewToils>c__Iterator1B;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
