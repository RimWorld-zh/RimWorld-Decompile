using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000B2 RID: 178
	public class JobGiver_AIFightEnemies : JobGiver_AIFightEnemy
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x000304F0 File Offset: 0x0002E8F0
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			bool allowManualCastWeapons = !pawn.IsColonist;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons);
			bool result;
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				result = false;
			}
			else
			{
				result = CastPositionFinder.TryFindCastPosition(new CastPositionRequest
				{
					caster = pawn,
					target = enemyTarget,
					verb = verb,
					maxRangeFromTarget = verb.verbProps.range,
					wantCoverFromTarget = (verb.verbProps.range > 5f)
				}, out dest);
			}
			return result;
		}
	}
}
