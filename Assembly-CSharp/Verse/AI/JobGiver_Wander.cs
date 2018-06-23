using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ACE RID: 2766
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x040026B9 RID: 9913
		protected float wanderRadius;

		// Token: 0x040026BA RID: 9914
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator = null;

		// Token: 0x040026BB RID: 9915
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x040026BC RID: 9916
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x040026BD RID: 9917
		protected Danger maxDanger = Danger.None;

		// Token: 0x040026BE RID: 9918
		protected int expiryInterval = -1;

		// Token: 0x06003D7A RID: 15738 RVA: 0x0003088C File Offset: 0x0002EC8C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Wander jobGiver_Wander = (JobGiver_Wander)base.DeepCopy(resolve);
			jobGiver_Wander.wanderRadius = this.wanderRadius;
			jobGiver_Wander.wanderDestValidator = this.wanderDestValidator;
			jobGiver_Wander.ticksBetweenWandersRange = this.ticksBetweenWandersRange;
			jobGiver_Wander.locomotionUrgency = this.locomotionUrgency;
			jobGiver_Wander.maxDanger = this.maxDanger;
			jobGiver_Wander.expiryInterval = this.expiryInterval;
			return jobGiver_Wander;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x000308F8 File Offset: 0x0002ECF8
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = pawn.CurJob != null && pawn.CurJob.def == JobDefOf.GotoWander;
			bool nextMoveOrderIsWait = pawn.mindState.nextMoveOrderIsWait;
			if (!flag)
			{
				pawn.mindState.nextMoveOrderIsWait = !pawn.mindState.nextMoveOrderIsWait;
			}
			Job result;
			if (nextMoveOrderIsWait && !flag)
			{
				result = new Job(JobDefOf.Wait_Wander)
				{
					expiryInterval = this.ticksBetweenWandersRange.RandomInRange
				};
			}
			else
			{
				IntVec3 exactWanderDest = this.GetExactWanderDest(pawn);
				if (!exactWanderDest.IsValid)
				{
					pawn.mindState.nextMoveOrderIsWait = false;
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.GotoWander, exactWanderDest)
					{
						locomotionUrgency = this.locomotionUrgency,
						expiryInterval = this.expiryInterval,
						checkOverrideOnExpire = true
					};
				}
			}
			return result;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x000309EC File Offset: 0x0002EDEC
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		// Token: 0x06003D7D RID: 15741
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);
	}
}
