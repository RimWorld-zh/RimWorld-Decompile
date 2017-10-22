using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		private bool attackMeleeThreatEvenIfNotHostile;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		protected abstract Pawn GetDefendee(Pawn pawn);

		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			return (!defendee.Spawned && defendee.CarriedBy == null) ? IntVec3.Invalid : defendee.PositionHeld;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			Pawn carriedBy = defendee.CarriedBy;
			Job result;
			if (carriedBy != null)
			{
				if (!pawn.CanReach((Thing)carriedBy, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result = null;
					goto IL_006f;
				}
				goto IL_0062;
			}
			if (defendee.Spawned && pawn.CanReach((Thing)defendee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				goto IL_0062;
			}
			result = null;
			goto IL_006f;
			IL_006f:
			return result;
			IL_0062:
			result = base.TryGiveJob(pawn);
			goto IL_006f;
		}

		protected override Thing FindAttackTarget(Pawn pawn)
		{
			Thing result;
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && pawn.CanReach((Thing)defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result = defendee.mindState.meleeThreat;
					goto IL_008b;
				}
			}
			result = base.FindAttackTarget(pawn);
			goto IL_008b;
			IL_008b:
			return result;
		}

		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Verb verb = pawn.TryGetAttackVerb(!pawn.IsColonist);
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
					target = pawn.mindState.enemyTarget,
					verb = verb,
					maxRangeFromTarget = 9999f,
					locus = this.GetDefendee(pawn).PositionHeld,
					maxRangeFromLocus = this.GetFlagRadius(pawn),
					wantCoverFromTarget = (verb.verbProps.range > 7.0),
					maxRegionsRadius = 50
				}, out dest);
			}
			return result;
		}
	}
}
