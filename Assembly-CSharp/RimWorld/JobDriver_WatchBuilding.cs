using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000061 RID: 97
	public class JobDriver_WatchBuilding : JobDriver
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0001BF50 File Offset: 0x0001A350
		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null))
			{
				result = false;
			}
			else if (!this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (base.TargetC.HasThing)
				{
					if (base.TargetC.Thing is Building_Bed)
					{
						if (!this.pawn.Reserve(this.job.targetC, this.job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null))
						{
							return false;
						}
					}
					else if (!this.pawn.Reserve(this.job.targetC, this.job, 1, -1, null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0001C070 File Offset: 0x0001A470
		public override bool CanBeginNowWhileLyingDown()
		{
			return base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, base.TargetC);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0001C0C4 File Offset: 0x0001A4C4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			bool hasBed = base.TargetC.HasThing && base.TargetC.Thing is Building_Bed;
			Toil watch;
			if (hasBed)
			{
				this.KeepLyingDown(TargetIndex.C);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.C);
				watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true);
				watch.AddFailCondition(() => !watch.actor.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				watch = new Toil();
			}
			watch.AddPreTickAction(delegate
			{
				this.WatchTickAction();
			});
			watch.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			watch.defaultCompleteMode = ToilCompleteMode.Delay;
			watch.defaultDuration = this.job.def.joyDuration;
			watch.handlingFacing = true;
			yield return watch;
			yield break;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0001C0F0 File Offset: 0x0001A4F0
		protected virtual void WatchTickAction()
		{
			this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			this.pawn.GainComfortFromCellIfPossible();
			Pawn pawn = this.pawn;
			Building joySource = (Building)base.TargetThingA;
			JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0001C148 File Offset: 0x0001A548
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
