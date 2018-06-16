using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000BC RID: 188
	public class JobGiver_AIGotoNearestHostile : ThinkNode_JobGiver
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x00032EBC File Offset: 0x000312BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			float num = float.MaxValue;
			Thing thing = null;
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(pawn))
				{
					Thing thing2 = (Thing)attackTarget;
					int num2 = thing2.Position.DistanceToSquared(pawn.Position);
					if ((float)num2 < num && pawn.CanReach(thing2, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						num = (float)num2;
						thing = thing2;
					}
				}
			}
			Job result;
			if (thing != null)
			{
				result = new Job(JobDefOf.Goto, thing)
				{
					checkOverrideOnExpire = true,
					expiryInterval = 500,
					collideWithPawns = true
				};
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
