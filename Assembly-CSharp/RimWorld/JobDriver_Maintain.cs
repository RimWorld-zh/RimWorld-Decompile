using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000073 RID: 115
	public class JobDriver_Maintain : JobDriver
	{
		// Token: 0x0400021E RID: 542
		private const int MaintainTicks = 180;

		// Token: 0x06000325 RID: 805 RVA: 0x00022488 File Offset: 0x00020888
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000224BC File Offset: 0x000208BC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil prepare = Toils_General.Wait(180);
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return prepare;
			Toil maintain = new Toil();
			maintain.initAction = delegate()
			{
				Pawn actor = maintain.actor;
				CompMaintainable compMaintainable = actor.CurJob.targetA.Thing.TryGetComp<CompMaintainable>();
				compMaintainable.Maintained();
			};
			maintain.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return maintain;
			yield break;
		}
	}
}
