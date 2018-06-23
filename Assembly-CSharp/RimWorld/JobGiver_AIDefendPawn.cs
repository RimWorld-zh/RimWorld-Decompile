using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000B3 RID: 179
	public abstract class JobGiver_AIDefendPawn : JobGiver_AIFightEnemy
	{
		// Token: 0x04000284 RID: 644
		private bool attackMeleeThreatEvenIfNotHostile;

		// Token: 0x06000448 RID: 1096 RVA: 0x00032820 File Offset: 0x00030C20
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIDefendPawn jobGiver_AIDefendPawn = (JobGiver_AIDefendPawn)base.DeepCopy(resolve);
			jobGiver_AIDefendPawn.attackMeleeThreatEvenIfNotHostile = this.attackMeleeThreatEvenIfNotHostile;
			return jobGiver_AIDefendPawn;
		}

		// Token: 0x06000449 RID: 1097
		protected abstract Pawn GetDefendee(Pawn pawn);

		// Token: 0x0600044A RID: 1098 RVA: 0x00032850 File Offset: 0x00030C50
		protected override IntVec3 GetFlagPosition(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			IntVec3 result;
			if (defendee.Spawned || defendee.CarriedBy != null)
			{
				result = defendee.PositionHeld;
			}
			else
			{
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00032894 File Offset: 0x00030C94
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn defendee = this.GetDefendee(pawn);
			Job result;
			if (defendee == null)
			{
				Log.Error(base.GetType() + " has null defendee. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				result = null;
			}
			else
			{
				Pawn carriedBy = defendee.CarriedBy;
				if (carriedBy != null)
				{
					if (!pawn.CanReach(carriedBy, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						return null;
					}
				}
				else if (!defendee.Spawned || !pawn.CanReach(defendee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return null;
				}
				result = base.TryGiveJob(pawn);
			}
			return result;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0003293C File Offset: 0x00030D3C
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			if (this.attackMeleeThreatEvenIfNotHostile)
			{
				Pawn defendee = this.GetDefendee(pawn);
				if (defendee.Spawned && !defendee.InMentalState && defendee.mindState.meleeThreat != null && defendee.mindState.meleeThreat != pawn && pawn.CanReach(defendee.mindState.meleeThreat, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return defendee.mindState.meleeThreat;
				}
			}
			return base.FindAttackTarget(pawn);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000329D8 File Offset: 0x00030DD8
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest)
		{
			Verb verb = pawn.TryGetAttackVerb(null, !pawn.IsColonist);
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
					wantCoverFromTarget = (verb.verbProps.range > 7f),
					maxRegions = 50
				}, out dest);
			}
			return result;
		}
	}
}
