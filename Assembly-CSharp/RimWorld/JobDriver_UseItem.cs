using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000087 RID: 135
	public class JobDriver_UseItem : JobDriver
	{
		// Token: 0x0600037F RID: 895 RVA: 0x000274B9 File Offset: 0x000258B9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000274D4 File Offset: 0x000258D4
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00027514 File Offset: 0x00025914
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00027548 File Offset: 0x00025948
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil prepare = Toils_General.Wait(this.useDuration);
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			prepare.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			prepare.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return prepare;
			Toil use = new Toil();
			use.initAction = delegate()
			{
				Pawn actor = use.actor;
				CompUsable compUsable = actor.CurJob.targetA.Thing.TryGetComp<CompUsable>();
				compUsable.UsedBy(actor);
			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return use;
			yield break;
		}

		// Token: 0x04000248 RID: 584
		private int useDuration = -1;
	}
}
