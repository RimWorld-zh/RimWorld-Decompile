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
			Job result;
			float radius;
			if (followee == null)
			{
				Log.Error(base.GetType() + "has null followee.");
				result = null;
			}
			else if (!GenAI.CanInteractPawn(pawn, followee))
			{
				result = null;
			}
			else
			{
				radius = this.GetRadius(pawn);
				if (followee.pather.Moving && (float)followee.pather.Destination.Cell.DistanceToSquared(pawn.Position) > radius * radius)
				{
					goto IL_00cb;
				}
				if (followee.GetRoom(RegionType.Set_Passable) != pawn.GetRoom(RegionType.Set_Passable) && !GenSight.LineOfSight(pawn.Position, followee.Position, followee.Map, false, null, 0, 0))
				{
					goto IL_00cb;
				}
				if ((float)followee.Position.DistanceToSquared(pawn.Position) > radius * radius)
					goto IL_00cb;
				result = null;
			}
			goto IL_01bb;
			IL_00cb:
			IntVec3 root = (!followee.pather.Moving || followee.pather.curPath == null) ? followee.Position : followee.pather.curPath.FinalWalkableNonDoorCell(followee.Map);
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(root, followee.Map, Mathf.RoundToInt((float)(radius * 0.699999988079071)), null);
			if (intVec == pawn.Position)
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.Goto, intVec);
				job.expiryInterval = this.FollowJobExpireInterval;
				job.checkOverrideOnExpire = true;
				if (((pawn.mindState.duty != null) ? pawn.mindState.duty.locomotion : LocomotionUrgency.None) != 0)
				{
					job.locomotionUrgency = pawn.mindState.duty.locomotion;
				}
				result = job;
			}
			goto IL_01bb;
			IL_01bb:
			return result;
		}
	}
}
