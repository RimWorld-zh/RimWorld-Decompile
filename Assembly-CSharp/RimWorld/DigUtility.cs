using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000BE RID: 190
	public static class DigUtility
	{
		// Token: 0x04000295 RID: 661
		private const int CheckOverrideInterval = 500;

		// Token: 0x06000478 RID: 1144 RVA: 0x00033258 File Offset: 0x00031658
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
				if (canMineMineables)
				{
					result = DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				else
				{
					result = DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
			}
			else
			{
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
					if (primaryVerb.verbProps.ai_IsBuildingDestroyer && (!primaryVerb.IsIncendiary() || blocker.FlammableNow))
					{
						return new Job(JobDefOf.UseVerbOnThing)
						{
							targetA = blocker,
							verbToUse = primaryVerb,
							expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange
						};
					}
				}
				if (canMineNonMineables)
				{
					result = DigUtility.MineOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
				else
				{
					result = DigUtility.MeleeOrWaitJob(pawn, blocker, cellBeforeBlocker);
				}
			}
			return result;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00033368 File Offset: 0x00031768
		private static Job MeleeOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			Job result;
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				result = DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			else
			{
				result = new Job(JobDefOf.AttackMelee, blocker)
				{
					ignoreDesignations = true,
					expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange,
					checkOverrideOnExpire = true
				};
			}
			return result;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000333D4 File Offset: 0x000317D4
		private static Job MineOrWaitJob(Pawn pawn, Thing blocker, IntVec3 cellBeforeBlocker)
		{
			Job result;
			if (!pawn.CanReserve(blocker, 1, -1, null, false))
			{
				result = DigUtility.WaitNearJob(pawn, cellBeforeBlocker);
			}
			else
			{
				result = new Job(JobDefOf.Mine, blocker)
				{
					ignoreDesignations = true,
					expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange,
					checkOverrideOnExpire = true
				};
			}
			return result;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00033440 File Offset: 0x00031840
		private static Job WaitNearJob(Pawn pawn, IntVec3 cellBeforeBlocker)
		{
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(cellBeforeBlocker, pawn.Map, 10, null);
			Job result;
			if (intVec == pawn.Position)
			{
				result = new Job(JobDefOf.Wait, 20, true);
			}
			else
			{
				result = new Job(JobDefOf.Goto, intVec, 500, true);
			}
			return result;
		}
	}
}
