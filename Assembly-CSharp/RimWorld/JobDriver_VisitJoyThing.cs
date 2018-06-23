using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200005F RID: 95
	public abstract class JobDriver_VisitJoyThing : JobDriver
	{
		// Token: 0x040001FC RID: 508
		protected const TargetIndex TargetThingIndex = TargetIndex.A;

		// Token: 0x060002BD RID: 701 RVA: 0x0001D5F0 File Offset: 0x0001B9F0
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0001D628 File Offset: 0x0001BA28
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil wait = Toils_General.Wait(this.job.def.joyDuration);
			wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			wait.tickAction = delegate()
			{
				this.WaitTickAction();
			};
			wait.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return wait;
			yield break;
		}

		// Token: 0x060002BF RID: 703
		protected abstract void WaitTickAction();
	}
}
