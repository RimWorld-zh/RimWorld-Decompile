using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		protected JobGiver_AIFollowPawn()
		{
		}

		protected abstract Pawn GetFollowee(Pawn pawn);

		protected abstract float GetRadius(Pawn pawn);

		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn followee = this.GetFollowee(pawn);
			Job result;
			if (followee == null)
			{
				Log.Error(base.GetType() + " has null followee. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				result = null;
			}
			else if (!followee.Spawned || !pawn.CanReach(followee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				float radius = this.GetRadius(pawn);
				if (!JobDriver_FollowClose.FarEnoughAndPossibleToStartJob(pawn, followee, radius))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.FollowClose, followee)
					{
						expiryInterval = this.FollowJobExpireInterval,
						checkOverrideOnExpire = true,
						followRadius = radius
					};
				}
			}
			return result;
		}
	}
}
