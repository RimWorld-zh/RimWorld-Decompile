using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A38 RID: 2616
	public class JobDriver_AttackMelee : JobDriver
	{
		// Token: 0x040024FE RID: 9470
		private int numMeleeAttacksMade = 0;

		// Token: 0x06003A13 RID: 14867 RVA: 0x001EB75C File Offset: 0x001E9B5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x001EB778 File Offset: 0x001E9B78
		public override bool TryMakePreToilReservations()
		{
			IAttackTarget attackTarget = this.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				this.pawn.Map.attackTargetReservationManager.Reserve(this.pawn, this.job, attackTarget);
			}
			return true;
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x001EB7CC File Offset: 0x001E9BCC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.DoAtomic(delegate
			{
				Pawn pawn = this.job.targetA.Thing as Pawn;
				if (pawn != null && pawn.Downed && this.pawn.mindState.duty != null && this.pawn.mindState.duty.attackDownedIfStarving && this.pawn.Starving())
				{
					this.job.killIncappedTarget = true;
				}
			});
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (this.pawn.meleeVerbs.TryMeleeAttack(thing, this.job.verbToUse, false))
				{
					if (this.pawn.CurJob != null && this.pawn.jobs.curDriver == this)
					{
						this.numMeleeAttacksMade++;
						if (this.numMeleeAttacksMade >= this.job.maxNumMeleeAttacks)
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
					}
				}
			}).FailOnDespawnedOrNull(TargetIndex.A);
			yield break;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x001EB7F8 File Offset: 0x001E9BF8
		public override void Notify_PatherFailed()
		{
			if (this.job.attackDoorIfTargetLost)
			{
				Thing thing;
				using (PawnPath pawnPath = base.Map.pathFinder.FindPath(this.pawn.Position, base.TargetA.Cell, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
				{
					if (!pawnPath.Found)
					{
						return;
					}
					IntVec3 intVec;
					thing = pawnPath.FirstBlockingBuilding(out intVec, this.pawn);
				}
				if (thing != null)
				{
					this.job.targetA = thing;
					this.job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
					this.job.expiryInterval = Rand.Range(2000, 4000);
					return;
				}
			}
			base.Notify_PatherFailed();
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x001EB8E8 File Offset: 0x001E9CE8
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}
	}
}
