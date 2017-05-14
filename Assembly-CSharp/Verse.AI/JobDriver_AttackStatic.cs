using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public class JobDriver_AttackStatic : JobDriver
	{
		private bool startedIncapacitated;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AttackStatic.<MakeNewToils>c__Iterator1B4 <MakeNewToils>c__Iterator1B = new JobDriver_AttackStatic.<MakeNewToils>c__Iterator1B4();
			<MakeNewToils>c__Iterator1B.<>f__this = this;
			JobDriver_AttackStatic.<MakeNewToils>c__Iterator1B4 expr_0E = <MakeNewToils>c__Iterator1B;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
