using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.AI
{
	public static class Toils_Combat
	{
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

		[CompilerGenerated]
		private sealed class <TrySetJobToUseAttackVerb>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex targetInd;

			public <TrySetJobToUseAttackVerb>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				bool allowManualCastWeapons = !actor.IsColonist;
				Verb verb = actor.TryGetAttackVerb(curJob.GetTarget(this.targetInd).Thing, allowManualCastWeapons);
				if (verb == null)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					curJob.verbToUse = verb;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GotoCastPosition>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex targetInd;

			internal bool closeIfDowned;

			internal float maxRangeFactor;

			public <GotoCastPosition>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(this.targetInd).Thing;
				Pawn pawn = thing as Pawn;
				IntVec3 intVec;
				if (!CastPositionFinder.TryFindCastPosition(new CastPositionRequest
				{
					caster = this.toil.actor,
					target = thing,
					verb = curJob.verbToUse,
					maxRangeFromTarget = ((this.closeIfDowned && pawn != null && pawn.Downed) ? Mathf.Min(curJob.verbToUse.verbProps.range, (float)pawn.RaceProps.executionRange) : Mathf.Max(curJob.verbToUse.verbProps.range * this.maxRangeFactor, 1.42f)),
					wantCoverFromTarget = false
				}, out intVec))
				{
					this.toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					this.toil.actor.pather.StartPath(intVec, PathEndMode.OnCell);
					actor.Map.pawnDestinationReservationManager.Reserve(actor, curJob, intVec);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <CastVerb>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex targetInd;

			internal bool canHitNonTargetPawns;

			public <CastVerb>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Verb verbToUse = this.toil.actor.jobs.curJob.verbToUse;
				LocalTargetInfo target = this.toil.actor.jobs.curJob.GetTarget(this.targetInd);
				bool flag = this.canHitNonTargetPawns;
				verbToUse.TryStartCastOn(target, false, flag);
			}
		}

		[CompilerGenerated]
		private sealed class <FollowAndMeleeAttack>c__AnonStorey3
		{
			internal Toil followAndAttack;

			internal TargetIndex targetInd;

			internal Action hitAction;

			public <FollowAndMeleeAttack>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.followAndAttack.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = curJob.GetTarget(this.targetInd).Thing;
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
						this.hitAction();
					}
				}
			}
		}
	}
}
