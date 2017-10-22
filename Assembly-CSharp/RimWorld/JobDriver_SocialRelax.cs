using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_SocialRelax : JobDriver
	{
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;

		private Thing GatherSpotParent
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private bool HasChair
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.B).HasThing;
			}
		}

		private bool HasDrink
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.C).HasThing;
			}
		}

		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(base.pawn.Position);
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (this.HasChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			}
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			if (this.HasDrink)
			{
				this.FailOnDestroyedNullOrForbidden(TargetIndex.C);
				yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
				yield return Toils_Haul.StartCarryThing(TargetIndex.C, false, false);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil chew = new Toil
			{
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator21)/*Error near IL_011b: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(((_003CMakeNewToils_003Ec__Iterator21)/*Error near IL_011b: stateMachine*/)._003C_003Ef__this.ClosestGatherSpotParentCell);
					((_003CMakeNewToils_003Ec__Iterator21)/*Error near IL_011b: stateMachine*/)._003C_003Ef__this.pawn.GainComfortFromCellIfPossible();
					JoyUtility.JoyTickCheckEnd(((_003CMakeNewToils_003Ec__Iterator21)/*Error near IL_011b: stateMachine*/)._003C_003Ef__this.pawn, JoyTickFullJoyAction.GoToNextToil, 1f);
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = base.CurJob.def.joyDuration
			};
			chew.AddFinishAction((Action)delegate
			{
				JoyUtility.TryGainRecRoomThought(((_003CMakeNewToils_003Ec__Iterator21)/*Error near IL_015e: stateMachine*/)._003C_003Ef__this.pawn);
			});
			chew.socialMode = RandomSocialMode.SuperActive;
			Toils_Ingest.AddIngestionEffects(chew, base.pawn, TargetIndex.C, TargetIndex.None);
			yield return chew;
			if (this.HasDrink)
			{
				yield return Toils_Ingest.FinalizeIngest(base.pawn, TargetIndex.C);
			}
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, closestGatherSpotParentCell, base.pawn);
		}
	}
}
