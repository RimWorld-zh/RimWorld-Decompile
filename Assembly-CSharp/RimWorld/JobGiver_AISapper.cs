using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000BD RID: 189
	public class JobGiver_AISapper : ThinkNode_JobGiver
	{
		// Token: 0x04000291 RID: 657
		private bool canMineMineables = true;

		// Token: 0x04000292 RID: 658
		private bool canMineNonMineables = true;

		// Token: 0x04000293 RID: 659
		private const float ReachDestDist = 10f;

		// Token: 0x04000294 RID: 660
		private const int CheckOverrideInterval = 500;

		// Token: 0x06000476 RID: 1142 RVA: 0x00032FB4 File Offset: 0x000313B4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AISapper jobGiver_AISapper = (JobGiver_AISapper)base.DeepCopy(resolve);
			jobGiver_AISapper.canMineMineables = this.canMineMineables;
			jobGiver_AISapper.canMineNonMineables = this.canMineNonMineables;
			return jobGiver_AISapper;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00032FF0 File Offset: 0x000313F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 intVec = (IntVec3)pawn.mindState.duty.focus;
			if (intVec.IsValid)
			{
				if ((float)intVec.DistanceToSquared(pawn.Position) < 100f && intVec.GetRoom(pawn.Map, RegionType.Set_Passable) == pawn.GetRoom(RegionType.Set_Passable) && intVec.WithinRegions(pawn.Position, pawn.Map, 9, TraverseMode.NoPassClosedDoors, RegionType.Set_Passable))
				{
					pawn.GetLord().Notify_ReachedDutyLocation(pawn);
					return null;
				}
			}
			if (!intVec.IsValid)
			{
				IAttackTarget attackTarget;
				if (!(from x in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
				where !x.ThreatDisabled(pawn) && x.Thing.Faction == Faction.OfPlayer && pawn.CanReach(x.Thing, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.PassAllDestroyableThings)
				select x).TryRandomElement(out attackTarget))
				{
					return null;
				}
				intVec = attackTarget.Thing.Position;
			}
			Job result;
			if (!pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.PassAllDestroyableThings))
			{
				result = null;
			}
			else
			{
				using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, intVec, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassAllDestroyableThings, false), PathEndMode.OnCell))
				{
					IntVec3 cellBeforeBlocker;
					Thing thing = pawnPath.FirstBlockingBuilding(out cellBeforeBlocker, pawn);
					if (thing != null)
					{
						Job job = DigUtility.PassBlockerJob(pawn, thing, cellBeforeBlocker, this.canMineMineables, this.canMineNonMineables);
						if (job != null)
						{
							return job;
						}
					}
				}
				result = new Job(JobDefOf.Goto, intVec, 500, true);
			}
			return result;
		}
	}
}
