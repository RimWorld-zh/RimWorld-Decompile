using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		protected abstract Pawn GetFollowee(Pawn pawn);

		protected abstract float GetRadius(Pawn pawn);

		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			if (followee == null)
			{
				Log.Error(base.GetType() + "has null followee.");
				return null;
			}
			if (!GenAI.CanInteractPawn(pawn, followee))
			{
				return null;
			}
			float radius = this.GetRadius(pawn);
			if (followee.pather.Moving && (float)followee.pather.Destination.Cell.DistanceToSquared(pawn.Position) > radius * radius)
			{
				goto IL_00bf;
			}
			if (followee.GetRoom(RegionType.Set_Passable) != pawn.GetRoom(RegionType.Set_Passable) && !GenSight.LineOfSight(pawn.Position, followee.Position, followee.Map, false, null, 0, 0))
			{
				goto IL_00bf;
			}
			if ((float)followee.Position.DistanceToSquared(pawn.Position) > radius * radius)
				goto IL_00bf;
			return null;
			IL_00bf:
			IntVec3 root = (!followee.pather.Moving || followee.pather.curPath == null) ? followee.Position : followee.pather.curPath.FinalWalkableNonDoorCell(followee.Map);
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(root, followee.Map, Mathf.RoundToInt((float)(radius * 0.699999988079071)), null);
			if (intVec == pawn.Position)
			{
				return null;
			}
			Job job = new Job(JobDefOf.Goto, intVec);
			job.expiryInterval = this.FollowJobExpireInterval;
			job.checkOverrideOnExpire = true;
			if (pawn.mindState.duty != null && pawn.mindState.duty.locomotion != 0)
			{
				job.locomotionUrgency = pawn.mindState.duty.locomotion;
			}
			return job;
		}
	}
}
