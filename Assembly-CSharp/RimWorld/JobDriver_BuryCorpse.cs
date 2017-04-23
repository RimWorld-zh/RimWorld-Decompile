using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_BuryCorpse : JobDriver
	{
		private const TargetIndex CorpseIndex = TargetIndex.A;

		private const TargetIndex GraveIndex = TargetIndex.B;

		private Corpse Corpse
		{
			get
			{
				return (Corpse)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BuryCorpse.<MakeNewToils>c__Iterator24 <MakeNewToils>c__Iterator = new JobDriver_BuryCorpse.<MakeNewToils>c__Iterator24();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_BuryCorpse.<MakeNewToils>c__Iterator24 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
