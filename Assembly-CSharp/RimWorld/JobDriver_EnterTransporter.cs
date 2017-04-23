using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_EnterTransporter : JobDriver
	{
		private TargetIndex TransporterInd = TargetIndex.A;

		private CompTransporter Transporter
		{
			get
			{
				Thing thing = base.CurJob.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnterTransporter.<MakeNewToils>c__Iterator29 <MakeNewToils>c__Iterator = new JobDriver_EnterTransporter.<MakeNewToils>c__Iterator29();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_EnterTransporter.<MakeNewToils>c__Iterator29 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
