using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_AttackMelee : JobDriver
	{
		private int numMeleeAttacksMade;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numMeleeAttacksMade, "numMeleeAttacksMade", 0, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_ReserveAttackTarget.TryReserve(TargetIndex.A);
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, (Action)delegate
			{
				Thing thing = ((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.CurJob.GetTarget(TargetIndex.A).Thing;
				if (((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.pawn.meleeVerbs.TryMeleeAttack(thing, ((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.CurJob.verbToUse, false) && ((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.pawn.CurJob != null && ((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.pawn.jobs.curDriver == ((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this)
				{
					((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.numMeleeAttacksMade++;
					if (((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.numMeleeAttacksMade >= ((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.pawn.CurJob.maxNumMeleeAttacks)
					{
						((_003CMakeNewToils_003Ec__Iterator1B5)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
					}
				}
			}).FailOnDespawnedOrNull(TargetIndex.A);
		}

		public override void Notify_PatherFailed()
		{
			if (base.CurJob.attackDoorIfTargetLost)
			{
				Thing thing = default(Thing);
				using (PawnPath pawnPath = base.Map.pathFinder.FindPath(base.pawn.Position, base.TargetA.Cell, TraverseParms.For(base.pawn, Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
				{
					if (pawnPath.Found)
					{
						IntVec3 intVec = default(IntVec3);
						thing = pawnPath.FirstBlockingBuilding(out intVec, base.pawn);
						goto end_IL_004e;
					}
					return;
					end_IL_004e:;
				}
				if (thing != null)
				{
					base.CurJob.targetA = thing;
					base.CurJob.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
					base.CurJob.expiryInterval = Rand.Range(2000, 4000);
					return;
				}
			}
			base.Notify_PatherFailed();
		}

		public override bool IsContinuation(Job j)
		{
			return base.CurJob.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}
	}
}
