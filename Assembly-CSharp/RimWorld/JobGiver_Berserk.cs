using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200010A RID: 266
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x040002E9 RID: 745
		private const float MaxAttackDistance = 30f;

		// Token: 0x040002EA RID: 746
		private const float WaitChance = 0.5f;

		// Token: 0x040002EB RID: 747
		private const int WaitTicks = 90;

		// Token: 0x040002EC RID: 748
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x040002ED RID: 749
		private const int MaxMeleeChaseTicks = 900;

		// Token: 0x06000587 RID: 1415 RVA: 0x0003C018 File Offset: 0x0003A418
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (Rand.Value < 0.5f)
			{
				result = new Job(JobDefOf.Wait_Combat)
				{
					expiryInterval = 90
				};
			}
			else if (pawn.TryGetAttackVerb(null, false) == null)
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = this.FindPawnTarget(pawn);
				if (pawn2 != null)
				{
					result = new Job(JobDefOf.AttackMelee, pawn2)
					{
						maxNumMeleeAttacks = 1,
						expiryInterval = Rand.Range(420, 900),
						canBash = true
					};
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0003C0B8 File Offset: 0x0003A4B8
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat, (Thing x) => x is Pawn, 0f, 30f, default(IntVec3), float.MaxValue, true);
		}
	}
}
