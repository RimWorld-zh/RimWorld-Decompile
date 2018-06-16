using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000055 RID: 85
	public class JobDriver_PlayBilliards : JobDriver
	{
		// Token: 0x06000293 RID: 659 RVA: 0x0001BB0C File Offset: 0x00019F0C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0001BB50 File Offset: 0x00019F50
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			Toil chooseCell = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
			yield return chooseCell;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil play = new Toil();
			play.initAction = delegate()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Walk;
			};
			play.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				if (this.ticksLeftThisToil == 300)
				{
					SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					base.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					Pawn pawn = this.pawn;
					Building joySource = (Building)base.TargetThingA;
					JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
				}
			};
			play.handlingFacing = true;
			play.socialMode = RandomSocialMode.SuperActive;
			play.defaultCompleteMode = ToilCompleteMode.Delay;
			play.defaultDuration = 600;
			play.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return play;
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return Toils_Jump.Jump(chooseCell);
			yield break;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001BB7C File Offset: 0x00019F7C
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		// Token: 0x040001F3 RID: 499
		private const int ShotDuration = 600;
	}
}
