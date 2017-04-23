using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_FillFermentingBarrel : JobDriver
	{
		private const TargetIndex BarrelInd = TargetIndex.A;

		private const TargetIndex WortInd = TargetIndex.B;

		private const int Duration = 200;

		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Thing Wort
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FillFermentingBarrel.<MakeNewToils>c__Iterator2A <MakeNewToils>c__Iterator2A = new JobDriver_FillFermentingBarrel.<MakeNewToils>c__Iterator2A();
			<MakeNewToils>c__Iterator2A.<>f__this = this;
			JobDriver_FillFermentingBarrel.<MakeNewToils>c__Iterator2A expr_0E = <MakeNewToils>c__Iterator2A;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
