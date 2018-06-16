using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC2 RID: 2754
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		// Token: 0x06003D45 RID: 15685 RVA: 0x0020520C File Offset: 0x0020360C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ExitMap jobGiver_ExitMap = (JobGiver_ExitMap)base.DeepCopy(resolve);
			jobGiver_ExitMap.defaultLocomotion = this.defaultLocomotion;
			jobGiver_ExitMap.jobMaxDuration = this.jobMaxDuration;
			jobGiver_ExitMap.canBash = this.canBash;
			jobGiver_ExitMap.forceCanDig = this.forceCanDig;
			jobGiver_ExitMap.forceCanDigIfAnyHostileActiveThreat = this.forceCanDigIfAnyHostileActiveThreat;
			jobGiver_ExitMap.forceCanDigIfCantReachMapEdge = this.forceCanDigIfCantReachMapEdge;
			jobGiver_ExitMap.failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan;
			return jobGiver_ExitMap;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00205284 File Offset: 0x00203684
		protected override Job TryGiveJob(Pawn pawn)
		{
			bool flag = false;
			if (this.forceCanDig || (pawn.mindState.duty != null && pawn.mindState.duty.canDig) || (this.forceCanDigIfCantReachMapEdge && !pawn.CanReachMapEdge()) || (this.forceCanDigIfAnyHostileActiveThreat && pawn.Faction != null && GenHostility.AnyHostileActiveThreatTo(pawn.Map, pawn.Faction)))
			{
				flag = true;
			}
			IntVec3 c;
			Job result;
			if (!this.TryFindGoodExitDest(pawn, flag, out c))
			{
				result = null;
			}
			else
			{
				if (flag)
				{
					using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, c, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false), PathEndMode.OnCell))
					{
						IntVec3 cellBeforeBlocker;
						Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
						if (thing != null)
						{
							Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, true, true);
							if (job != null)
							{
								return job;
							}
						}
					}
				}
				result = new Job(JobDefOf.Goto, c)
				{
					exitMapOnArrival = true,
					failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan,
					locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.defaultLocomotion, LocomotionUrgency.Jog),
					expiryInterval = this.jobMaxDuration,
					canBash = this.canBash
				};
			}
			return result;
		}

		// Token: 0x06003D47 RID: 15687
		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 dest);

		// Token: 0x040026A8 RID: 9896
		protected LocomotionUrgency defaultLocomotion = LocomotionUrgency.None;

		// Token: 0x040026A9 RID: 9897
		protected int jobMaxDuration = 999999;

		// Token: 0x040026AA RID: 9898
		protected bool canBash = false;

		// Token: 0x040026AB RID: 9899
		protected bool forceCanDig = false;

		// Token: 0x040026AC RID: 9900
		protected bool forceCanDigIfAnyHostileActiveThreat = false;

		// Token: 0x040026AD RID: 9901
		protected bool forceCanDigIfCantReachMapEdge = false;

		// Token: 0x040026AE RID: 9902
		protected bool failIfCantJoinOrCreateCaravan = false;
	}
}
