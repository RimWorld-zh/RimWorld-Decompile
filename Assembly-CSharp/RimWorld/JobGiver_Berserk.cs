using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		private const float MaxAttackDistance = 30f;

		private const float WaitChance = 0.5f;

		private const int WaitTicks = 90;

		private const int MinMeleeChaseTicks = 420;

		private const int MaxMeleeChaseTicks = 900;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (Rand.Value < 0.5)
			{
				Job job = new Job(JobDefOf.WaitCombat);
				job.expiryInterval = 90;
				result = job;
			}
			else if (pawn.TryGetAttackVerb(false) == null)
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = this.FindPawnTarget(pawn);
				if (pawn2 != null)
				{
					Job job2 = new Job(JobDefOf.AttackMelee, (Thing)pawn2);
					job2.maxNumMeleeAttacks = 1;
					job2.expiryInterval = Rand.Range(420, 900);
					job2.canBash = true;
					result = job2;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Predicate<Thing>)((Thing x) => x is Pawn), 0f, 30f, default(IntVec3), 3.40282347E+38f, true);
		}
	}
}
