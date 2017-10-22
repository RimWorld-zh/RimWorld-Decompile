using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class DigUtility
	{
		private const int CheckOverrideInterval = 500;

		public static Job PassBlockerJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker, bool canMineMineables, bool canMineNonMineables)
		{
			if (StatDefOf.MiningSpeed.Worker.IsDisabledFor(pawn))
			{
				canMineMineables = false;
				canMineNonMineables = false;
			}
			Job result;
			if (blocker.def.mineable)
			{
				result = ((!canMineMineables) ? DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker) : DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker));
			}
			else
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
					if (primaryVerb.verbProps.ai_IsBuildingDestroyer && (!primaryVerb.IsIncendiary() || blocker.FlammableNow))
					{
						Job job = new Job(JobDefOf.UseVerbOnThing);
						job.targetA = blocker;
						job.verbToUse = primaryVerb;
						job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
						result = job;
						goto IL_0102;
					}
				}
				result = ((!canMineNonMineables) ? DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker) : DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker));
			}
			goto IL_0102;
			IL_0102:
			return result;
		}

		private static Job MeleeOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			Job result;
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				result = DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			else
			{
				Job job = new Job(JobDefOf.AttackMelee, blocker);
				job.ignoreDesignations = true;
				job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
				job.checkOverrideOnExpire = true;
				result = job;
			}
			return result;
		}

		private static Job MineOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			Job result;
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				result = DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			else
			{
				Job job = new Job(JobDefOf.Mine, blocker);
				job.ignoreDesignations = true;
				job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
				job.checkOverrideOnExpire = true;
				result = job;
			}
			return result;
		}

		private static Job WaitNearJob(Pawn pawn, IntVec3 cellBeforeBlocker)
		{
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(cellBeforeBlocker, pawn.Map, 10, null);
			return (!(intVec == pawn.Position)) ? new Job(JobDefOf.Goto, intVec, 500, true) : new Job(JobDefOf.Wait, 20, true);
		}
	}
}
