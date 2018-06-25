using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_AIDefendPoint : JobGiver_AIFightEnemy
	{
		public JobGiver_AIDefendPoint()
		{
		}

		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Verb verb = pawn.TryGetAttackVerb(enemyTarget, !pawn.IsColonist);
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
					maxRangeFromTarget = 9999f,
					locus = (IntVec3)pawn.mindState.duty.focus,
					maxRangeFromLocus = pawn.mindState.duty.radius,
					wantCoverFromTarget = (verb.verbProps.range > 7f)
				}, out dest);
			}
			return result;
		}
	}
}
