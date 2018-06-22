using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200005A RID: 90
	public class JobDriver_SitFacingBuilding : JobDriver
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0001C548 File Offset: 0x0001A948
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null) && this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0001C5B4 File Offset: 0x0001A9B4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			Toil play = new Toil();
			play.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
				this.pawn.GainComfortFromCellIfPossible();
				Pawn pawn = this.pawn;
				Building joySource = (Building)base.TargetThingA;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
			};
			play.handlingFacing = true;
			play.defaultCompleteMode = ToilCompleteMode.Delay;
			play.defaultDuration = this.job.def.joyDuration;
			play.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			this.ModifyPlayToil(play);
			yield return play;
			yield break;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0001C5DE File Offset: 0x0001A9DE
		protected virtual void ModifyPlayToil(Toil toil)
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0001C5E4 File Offset: 0x0001A9E4
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}
	}
}
