using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Manhunter : ThinkNode_JobGiver
	{
		private const float WaitChance = 0.75f;

		private const int WaitTicks = 90;

		private const int MinMeleeChaseTicks = 420;

		private const int MaxMeleeChaseTicks = 900;

		private const int WanderOutsideDoorRegions = 9;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.TryGetAttackVerb(false) == null)
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = this.FindPawnTarget(pawn);
				if (pawn2 != null && pawn.CanReach((Thing)pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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
								IntVec3 loc = default(IntVec3);
								if (!pawnPath.TryFindLastCellBeforeBlockingDoor(pawn, out loc))
								{
									Log.Error(pawn + " did TryFindLastCellBeforeDoor but found none when it should have been one. Target: " + pawn2.LabelCap);
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

		private Job MeleeAttackJob(Pawn pawn, Thing target)
		{
			Job job = new Job(JobDefOf.AttackMelee, target);
			job.maxNumMeleeAttacks = 1;
			job.expiryInterval = Rand.Range(420, 900);
			job.attackDoorIfTargetLost = true;
			return job;
		}

		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat, (Predicate<Thing>)((Thing x) => x is Pawn && (int)x.def.race.intelligence >= 1), 0f, 9999f, default(IntVec3), 3.40282347E+38f, true);
		}

		private Building FindTurretTarget(Pawn pawn)
		{
			return (Building)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Predicate<Thing>)((Thing t) => t is Building), 0f, 70f, default(IntVec3), 3.40282347E+38f, false);
		}
	}
}
