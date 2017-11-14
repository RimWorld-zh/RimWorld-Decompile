using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_AIDefendPoint : JobGiver_AIFightEnemy
	{
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Verb verb = pawn.TryGetAttackVerb(!pawn.IsColonist);
			if (verb == null)
			{
				dest = IntVec3.Invalid;
				return false;
			}
			CastPositionRequest newReq = default(CastPositionRequest);
			newReq.caster = pawn;
			newReq.target = pawn.mindState.enemyTarget;
			newReq.verb = verb;
			newReq.maxRangeFromTarget = 9999f;
			newReq.locus = (IntVec3)pawn.mindState.duty.focus;
			newReq.maxRangeFromLocus = pawn.mindState.duty.radius;
			newReq.wantCoverFromTarget = (verb.verbProps.range > 7.0);
			return CastPositionFinder.TryFindCastPosition(newReq, out dest);
		}
	}
}
