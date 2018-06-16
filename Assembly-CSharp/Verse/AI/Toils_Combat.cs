using System;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A4C RID: 2636
	public static class Toils_Combat
	{
		// Token: 0x06003AB0 RID: 15024 RVA: 0x001F1B84 File Offset: 0x001EFF84
		public static Toil TrySetJobToUseAttackVerb(TargetIndex targetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				bool allowManualCastWeapons = !actor.IsColonist;
				Verb verb = actor.TryGetAttackVerb(curJob.GetTarget(targetInd).Thing, allowManualCastWeapons);
				if (verb == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					curJob.verbToUse = verb;
				}
			};
			return toil;
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x001F1BD0 File Offset: 0x001EFFD0
		public static Toil GotoCastPosition(TargetIndex targetInd, bool closeIfDowned = false, float maxRangeFactor = 1f)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(targetInd).Thing;
				Pawn pawn = thing as Pawn;
				IntVec3 intVec;
				if (!CastPositionFinder.TryFindCastPosition(new CastPositionRequest
				{
					caster = toil.actor,
					target = thing,
					verb = curJob.verbToUse,
					maxRangeFromTarget = ((closeIfDowned && pawn != null && pawn.Downed) ? Mathf.Min(curJob.verbToUse.verbProps.range, (float)pawn.RaceProps.executionRange) : Mathf.Max(curJob.verbToUse.verbProps.range * maxRangeFactor, 1.42f)),
					wantCoverFromTarget = false
				}, out intVec))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					toil.actor.pather.StartPath(intVec, PathEndMode.OnCell);
					actor.Map.pawnDestinationReservationManager.Reserve(actor, curJob, intVec);
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x001F1C48 File Offset: 0x001F0048
		public static Toil CastVerb(TargetIndex targetInd, bool canHitNonTargetPawns = true)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Verb verbToUse = toil.actor.jobs.curJob.verbToUse;
				LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(targetInd);
				bool canHitNonTargetPawns2 = canHitNonTargetPawns;
				verbToUse.TryStartCastOn(target, false, canHitNonTargetPawns2);
			};
			toil.defaultCompleteMode = ToilCompleteMode.FinishedBusy;
			return toil;
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x001F1CA8 File Offset: 0x001F00A8
		public static Toil FollowAndMeleeAttack(TargetIndex targetInd, Action hitAction)
		{
			Toil followAndAttack = new Toil();
			followAndAttack.tickAction = delegate()
			{
				Pawn actor = followAndAttack.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = curJob.GetTarget(targetInd).Thing;
				Pawn pawn = thing as Pawn;
				if (!thing.Spawned)
				{
					curDriver.ReadyForNextToil();
				}
				else if (thing != actor.pather.Destination.Thing || (!actor.pather.Moving && !actor.CanReachImmediate(thing, PathEndMode.Touch)))
				{
					actor.pather.StartPath(thing, PathEndMode.Touch);
				}
				else if (actor.CanReachImmediate(thing, PathEndMode.Touch))
				{
					if (pawn != null && pawn.Downed && !curJob.killIncappedTarget)
					{
						curDriver.ReadyForNextToil();
					}
					else
					{
						hitAction();
					}
				}
			};
			followAndAttack.defaultCompleteMode = ToilCompleteMode.Never;
			return followAndAttack;
		}
	}
}
