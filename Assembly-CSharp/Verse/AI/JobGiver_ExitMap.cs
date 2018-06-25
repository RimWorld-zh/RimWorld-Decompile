using System;
using RimWorld;

namespace Verse.AI
{
	public abstract class JobGiver_ExitMap : ThinkNode_JobGiver
	{
		protected LocomotionUrgency defaultLocomotion = LocomotionUrgency.None;

		protected int jobMaxDuration = 999999;

		protected bool canBash = false;

		protected bool forceCanDig = false;

		protected bool forceCanDigIfAnyHostileActiveThreat = false;

		protected bool forceCanDigIfCantReachMapEdge = false;

		protected bool failIfCantJoinOrCreateCaravan = false;

		protected JobGiver_ExitMap()
		{
		}

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

		protected abstract bool TryFindGoodExitDest(Pawn pawn, bool canDig, out IntVec3 dest);
	}
}
