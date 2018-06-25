using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000110 RID: 272
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		// Token: 0x040002F3 RID: 755
		private const float WaitChance = 0.75f;

		// Token: 0x040002F4 RID: 756
		private const int WaitTicks = 90;

		// Token: 0x040002F5 RID: 757
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x040002F6 RID: 758
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x040002F7 RID: 759
		private const int WanderOutsideDoorRegions = 9;

		// Token: 0x06000598 RID: 1432 RVA: 0x0003C62C File Offset: 0x0003AA2C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = this.FindPawnTarget(pawn);
				if (pawn2 != null && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result = this.MeleeAttackJob(pawn, pawn2);
				}
				else
				{
					Building building = this.FindTurretTarget(pawn);
					if (building != null)
					{
						result = this.MeleeAttackJob(pawn, building);
					}
					else
					{
						if (pawn2 != null)
						{
							using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, pawn2.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
							{
								if (!pawnPath.Found)
								{
									return null;
								}
								IntVec3 loc;
								if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
								{
									Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap, false);
									return null;
								}
								IntVec3 randomCell = CellFinder.RandomRegionNear(loc.GetRegion(pawn.Map, RegionType.Set_Passable), 9, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable).RandomCell;
								if (randomCell == pawn.Position)
								{
									return new Job(JobDefOf.Wait, 30, false);
								}
								return new Job(JobDefOf.Goto, randomCell);
							}
						}
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0003C798 File Offset: 0x0003AB98
		private Job MeleeAttackJob(Pawn pawn, Thing target)
		{
			return new Job(JobDefOf.AttackMelee, target)
			{
				maxNumMeleeAttacks = 1,
				expiryInterval = Rand.Range(420, 900),
				attackDoorIfTargetLost = true
			};
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0003C7E4 File Offset: 0x0003ABE4
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, true);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0003C83C File Offset: 0x0003AC3C
		private Building FindTurretTarget(Pawn pawn)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Thing t) => t is Building, 0f, 70f, default(IntVec3), float.MaxValue, false);
		}
	}
}
