using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000B9 RID: 185
	public abstract class JobGiver_AIFollowPawn : ThinkNode_JobGiver
	{
		// Token: 0x06000467 RID: 1127
		protected abstract Pawn GetFollowee(Pawn pawn);

		// Token: 0x06000468 RID: 1128
		protected abstract float GetRadius(Pawn pawn);

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00032CBC File Offset: 0x000310BC
		protected virtual int FollowJobExpireInterval
		{
			get
			{
				return 140;
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00032CD8 File Offset: 0x000310D8
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
