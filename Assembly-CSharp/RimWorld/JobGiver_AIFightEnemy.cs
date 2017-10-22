#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
	{
		private float targetAcquireRadius = 56f;

		private float targetKeepRadius = 65f;

		private bool needLOSToAcquireNonPawnTargets = false;

		private bool chaseTarget = false;

		public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550);

		private static readonly IntRange ExpiryInterval_Melee = new IntRange(360, 480);

		private const int MinTargetDistanceToMove = 5;

		private const int TicksSinceEngageToLoseTarget = 400;

		protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest);

		protected virtual float GetFlagRadius(Pawn pawn)
		{
			return 999999f;
		}

		protected virtual IntVec3 GetFlagPosition(Pawn pawn)
		{
			return IntVec3.Invalid;
		}

		protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			return true;
		}

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
			jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
			jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
			jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
			jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
			return jobGiver_AIFightEnemy;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			this.UpdateEnemyTarget(pawn);
			Thing enemyTarget = pawn.mindState.enemyTarget;
			Job result;
			if (enemyTarget == null)
			{
				result = null;
			}
			else
			{
				bool allowManualCastWeapons = !pawn.IsColonist;
				Verb verb = pawn.TryGetAttackVerb(allowManualCastWeapons);
				if (verb == null)
				{
					result = null;
				}
				else if (verb.verbProps.MeleeRange)
				{
					result = this.MeleeAttackJob(enemyTarget);
				}
				else
				{
					bool flag = CoverUtility.CalculateOverallBlockChance(pawn.Position, enemyTarget.Position, pawn.Map) > 0.0099999997764825821;
					bool flag2 = pawn.Position.Standable(pawn.Map);
					bool flag3 = verb.CanHitTarget(enemyTarget);
					bool flag4 = (pawn.Position - enemyTarget.Position).LengthHorizontalSquared < 25;
					if (flag && flag2 && flag3)
					{
						goto IL_00e3;
					}
					if (flag4 && flag3)
						goto IL_00e3;
					IntVec3 intVec = default(IntVec3);
					if (!this.TryFindShootingPosition(pawn, out intVec))
					{
						result = null;
					}
					else if (intVec == pawn.Position)
					{
						result = new Job(JobDefOf.WaitCombat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
					}
					else
					{
						Job job = new Job(JobDefOf.Goto, intVec);
						job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange;
						job.checkOverrideOnExpire = true;
						result = job;
					}
				}
			}
			goto IL_0185;
			IL_00e3:
			result = new Job(JobDefOf.WaitCombat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
			goto IL_0185;
			IL_0185:
			return result;
		}

		protected virtual Job MeleeAttackJob(Thing enemyTarget)
		{
			Job job = new Job(JobDefOf.AttackMelee, enemyTarget);
			job.expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_Melee.RandomInRange;
			job.checkOverrideOnExpire = true;
			job.expireRequiresEnemiesNearby = true;
			return job;
		}

		protected virtual void UpdateEnemyTarget(Pawn pawn)
		{
			Profiler.BeginSample("UpdateEnemyTarget");
			Thing thing = pawn.mindState.enemyTarget;
			if (thing != null && (thing.Destroyed || Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > 400 || !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (float)(pawn.Position - thing.Position).LengthHorizontalSquared > this.targetKeepRadius * this.targetKeepRadius || ((IAttackTarget)thing).ThreatDisabled()))
			{
				thing = null;
			}
			if (thing == null)
			{
				thing = this.FindAttackTargetIfPossible(pawn);
				if (thing != null)
				{
					pawn.mindState.Notify_EngagedTarget();
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnAcquiredTarget(pawn, thing);
					}
				}
			}
			else
			{
				Thing thing2 = this.FindAttackTargetIfPossible(pawn);
				if (thing2 == null && !this.chaseTarget)
				{
					thing = null;
				}
				else if (thing2 != null && thing2 != thing)
				{
					pawn.mindState.Notify_EngagedTarget();
					thing = thing2;
				}
			}
			pawn.mindState.enemyTarget = thing;
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null && pawn2.Faction == Faction.OfPlayer && pawn.Position.InHorDistOf(pawn2.Position, 30f))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			Profiler.EndSample();
		}

		private Thing FindAttackTargetIfPossible(Pawn pawn)
		{
			return (pawn.TryGetAttackVerb(!pawn.IsColonist) != null) ? this.FindAttackTarget(pawn) : null;
		}

		protected virtual Thing FindAttackTarget(Pawn pawn)
		{
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat;
			if (this.needLOSToAcquireNonPawnTargets)
			{
				targetScanFlags = (TargetScanFlags)(byte)((int)targetScanFlags | 2);
			}
			if (this.PrimaryVerbIsIncendiary(pawn))
			{
				targetScanFlags = (TargetScanFlags)(byte)((int)targetScanFlags | 16);
			}
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, (Predicate<Thing>)((Thing x) => this.ExtraTargetValidator(pawn, x)), 0f, this.targetAcquireRadius, this.GetFlagPosition(pawn), this.GetFlagRadius(pawn), false);
		}

		private bool PrimaryVerbIsIncendiary(Pawn pawn)
		{
			List<Verb> allVerbs;
			int i;
			if (pawn.equipment != null && pawn.equipment.Primary != null)
			{
				allVerbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
				for (i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.isPrimary)
						goto IL_0051;
				}
			}
			bool result = false;
			goto IL_007c;
			IL_007c:
			return result;
			IL_0051:
			result = allVerbs[i].IsIncendiary();
			goto IL_007c;
		}
	}
}
