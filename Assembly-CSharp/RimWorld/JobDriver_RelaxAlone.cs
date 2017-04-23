using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RelaxAlone : JobDriver
	{
		private Rot4 faceDir;

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RelaxAlone.<MakeNewToils>c__Iterator1E <MakeNewToils>c__Iterator1E = new JobDriver_RelaxAlone.<MakeNewToils>c__Iterator1E();
			<MakeNewToils>c__Iterator1E.<>f__this = this;
			JobDriver_RelaxAlone.<MakeNewToils>c__Iterator1E expr_0E = <MakeNewToils>c__Iterator1E;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Rot4>(ref this.faceDir, "faceDir", default(Rot4), false);
		}
	}
}
