using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AD1 RID: 2769
	public abstract class JobGiver_Wander : ThinkNode_JobGiver
	{
		// Token: 0x040026C1 RID: 9921
		protected float wanderRadius;

		// Token: 0x040026C2 RID: 9922
		protected Func<Pawn, IntVec3, IntVec3, bool> wanderDestValidator = null;

		// Token: 0x040026C3 RID: 9923
		protected IntRange ticksBetweenWandersRange = new IntRange(20, 100);

		// Token: 0x040026C4 RID: 9924
		protected LocomotionUrgency locomotionUrgency = LocomotionUrgency.Walk;

		// Token: 0x040026C5 RID: 9925
		protected Danger maxDanger = Danger.None;

		// Token: 0x040026C6 RID: 9926
		protected int expiryInterval = -1;

		// Token: 0x06003D7E RID: 15742 RVA: 0x000308A8 File Offset: 0x0002ECA8
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

		// Token: 0x06003D7F RID: 15743 RVA: 0x00030914 File Offset: 0x0002ED14
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

		// Token: 0x06003D80 RID: 15744 RVA: 0x00030A08 File Offset: 0x0002EE08
		protected virtual IntVec3 GetExactWanderDest(Pawn pawn)
		{
			IntVec3 wanderRoot = this.GetWanderRoot(pawn);
			return RCellFinder.RandomWanderDestFor(pawn, wanderRoot, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		// Token: 0x06003D81 RID: 15745
		protected abstract IntVec3 GetWanderRoot(Pawn pawn);
	}
}
