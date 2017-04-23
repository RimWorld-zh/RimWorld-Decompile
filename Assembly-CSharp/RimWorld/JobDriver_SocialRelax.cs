using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_SocialRelax.<MakeNewToils>c__Iterator21 <MakeNewToils>c__Iterator = new JobDriver_SocialRelax.<MakeNewToils>c__Iterator21();
			<MakeNewToils>c__Iterator.<>f__this = this;
			JobDriver_SocialRelax.<MakeNewToils>c__Iterator21 expr_0E = <MakeNewToils>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, closestGatherSpotParentCell, this.pawn);
		}
	}
}
