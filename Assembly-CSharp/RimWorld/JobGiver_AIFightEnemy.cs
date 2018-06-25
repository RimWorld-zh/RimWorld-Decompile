using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000B8 RID: 184
	public abstract class JobGiver_AIFightEnemy : ThinkNode_JobGiver
	{
		// Token: 0x04000287 RID: 647
		private float targetAcquireRadius = 56f;

		// Token: 0x04000288 RID: 648
		private float targetKeepRadius = 65f;

		// Token: 0x04000289 RID: 649
		private bool needLOSToAcquireNonPawnTargets = false;

		// Token: 0x0400028A RID: 650
		private bool chaseTarget = false;

		// Token: 0x0400028B RID: 651
		public static readonly IntRange ExpiryInterval_ShooterSucceeded = new IntRange(450, 550);

		// Token: 0x0400028C RID: 652
		private static readonly IntRange ExpiryInterval_Melee = new IntRange(360, 480);

		// Token: 0x0400028D RID: 653
		private const int MinTargetDistanceToMove = 5;

		// Token: 0x0400028E RID: 654
		private const int TicksSinceEngageToLoseTarget = 400;

		// Token: 0x0600045A RID: 1114
		protected abstract bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest);

		// Token: 0x0600045B RID: 1115 RVA: 0x0002FF28 File Offset: 0x0002E328
		protected virtual float GetFlagRadius(Pawn pawn)
		{
			return 999999f;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0002FF44 File Offset: 0x0002E344
		protected virtual IntVec3 GetFlagPosition(Pawn pawn)
		{
			return IntVec3.Invalid;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0002FF60 File Offset: 0x0002E360
		protected virtual bool ExtraTargetValidator(Pawn pawn, Thing target)
		{
			return true;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0002FF78 File Offset: 0x0002E378
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AIFightEnemy jobGiver_AIFightEnemy = (JobGiver_AIFightEnemy)base.DeepCopy(resolve);
			jobGiver_AIFightEnemy.targetAcquireRadius = this.targetAcquireRadius;
			jobGiver_AIFightEnemy.targetKeepRadius = this.targetKeepRadius;
			jobGiver_AIFightEnemy.needLOSToAcquireNonPawnTargets = this.needLOSToAcquireNonPawnTargets;
			jobGiver_AIFightEnemy.chaseTarget = this.chaseTarget;
			return jobGiver_AIFightEnemy;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0002FFCC File Offset: 0x0002E3CC
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
				Verb verb = pawn.TryGetAttackVerb(enemyTarget, allowManualCastWeapons);
				if (verb == null)
				{
					result = null;
				}
				else if (verb.verbProps.IsMeleeAttack)
				{
					result = this.MeleeAttackJob(enemyTarget);
				}
				else
				{
					bool flag = CoverUtility.CalculateOverallBlockChance(pawn, enemyTarget.Position, pawn.Map) > 0.01f;
					bool flag2 = pawn.Position.Standable(pawn.Map);
					bool flag3 = verb.CanHitTarget(enemyTarget);
					bool flag4 = (pawn.Position - enemyTarget.Position).LengthHorizontalSquared < 25;
					IntVec3 intVec;
					if ((flag && flag2 && flag3) || (flag4 && flag3))
					{
						result = new Job(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
					}
					else if (!this.TryFindShootingPosition(pawn, out intVec))
					{
						result = null;
					}
					else if (intVec == pawn.Position)
					{
						result = new Job(JobDefOf.Wait_Combat, JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange, true);
					}
					else
					{
						result = new Job(JobDefOf.Goto, intVec)
						{
							expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_ShooterSucceeded.RandomInRange,
							checkOverrideOnExpire = true
						};
					}
				}
			}
			return result;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00030160 File Offset: 0x0002E560
		protected virtual Job MeleeAttackJob(Thing enemyTarget)
		{
			return new Job(JobDefOf.AttackMelee, enemyTarget)
			{
				expiryInterval = JobGiver_AIFightEnemy.ExpiryInterval_Melee.RandomInRange,
				checkOverrideOnExpire = true,
				expireRequiresEnemiesNearby = true
			};
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000301A8 File Offset: 0x0002E5A8
		protected virtual void UpdateEnemyTarget(Pawn pawn)
		{
			Profiler.BeginSample("UpdateEnemyTarget");
			Thing thing = pawn.mindState.enemyTarget;
			if (thing != null)
			{
				if (thing.Destroyed || Find.TickManager.TicksGame - pawn.mindState.lastEngageTargetTick > 400 || !pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (float)(pawn.Position - thing.Position).LengthHorizontalSquared > this.targetKeepRadius * this.targetKeepRadius || ((IAttackTarget)thing).ThreatDisabled(pawn))
				{
					thing = null;
				}
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

		// Token: 0x06000462 RID: 1122 RVA: 0x00030330 File Offset: 0x0002E730
		private Thing FindAttackTargetIfPossible(Pawn pawn)
		{
			Thing result;
			if (pawn.TryGetAttackVerb(null, !pawn.IsColonist) == null)
			{
				result = null;
			}
			else
			{
				result = this.FindAttackTarget(pawn);
			}
			return result;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00030368 File Offset: 0x0002E768
		protected virtual Thing FindAttackTarget(Pawn pawn)
		{
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat;
			if (this.needLOSToAcquireNonPawnTargets)
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToNonPawns;
			}
			if (this.PrimaryVerbIsIncendiary(pawn))
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, (Thing x) => this.ExtraTargetValidator(pawn, x), 0f, this.targetAcquireRadius, this.GetFlagPosition(pawn), this.GetFlagRadius(pawn), false);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000303FC File Offset: 0x0002E7FC
		private bool PrimaryVerbIsIncendiary(Pawn pawn)
		{
			if (pawn.equipment != null && pawn.equipment.Primary != null)
			{
				List<Verb> allVerbs = pawn.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.isPrimary)
					{
						return allVerbs[i].IsIncendiary();
					}
				}
			}
			return false;
		}
	}
}
