using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200005C RID: 92
	public class JobDriver_SocialRelax : JobDriver
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0001D15C File Offset: 0x0001B55C
		private Thing GatherSpotParent
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0001D188 File Offset: 0x0001B588
		private bool HasChair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).HasThing;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0001D1B4 File Offset: 0x0001B5B4
		private bool HasDrink
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).HasThing;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0001D1E0 File Offset: 0x0001B5E0
		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001D214 File Offset: 0x0001B614
		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (this.HasDrink)
				{
					if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001D290 File Offset: 0x0001B690
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (this.HasChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			}
			if (this.HasDrink)
			{
				this.FailOnDestroyedNullOrForbidden(TargetIndex.C);
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
				yield return Toils_Haul.StartCarryThing(TargetIndex.C, false, false, false);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil chew = new Toil();
			chew.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(this.ClosestGatherSpotParentCell);
				this.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.GoToNextToil, 1f, null);
			};
			chew.handlingFacing = true;
			chew.defaultCompleteMode = ToilCompleteMode.Delay;
			chew.defaultDuration = this.job.def.joyDuration;
			chew.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			chew.socialMode = RandomSocialMode.SuperActive;
			Toils_Ingest.AddIngestionEffects(chew, this.pawn, TargetIndex.C, TargetIndex.None);
			yield return chew;
			if (this.HasDrink)
			{
				yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.C);
			}
			yield break;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0001D2BC File Offset: 0x0001B6BC
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, closestGatherSpotParentCell, this.pawn);
		}

		// Token: 0x040001F9 RID: 505
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		// Token: 0x040001FA RID: 506
		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		// Token: 0x040001FB RID: 507
		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;
	}
}
