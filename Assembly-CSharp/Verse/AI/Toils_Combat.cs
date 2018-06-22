using System;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A48 RID: 2632
	public static class Toils_Combat
	{
		// Token: 0x06003AAD RID: 15021 RVA: 0x001F1F50 File Offset: 0x001F0350
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

		// Token: 0x06003AAE RID: 15022 RVA: 0x001F1F9C File Offset: 0x001F039C
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

		// Token: 0x06003AAF RID: 15023 RVA: 0x001F2014 File Offset: 0x001F0414
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

		// Token: 0x06003AB0 RID: 15024 RVA: 0x001F2074 File Offset: 0x001F0474
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
