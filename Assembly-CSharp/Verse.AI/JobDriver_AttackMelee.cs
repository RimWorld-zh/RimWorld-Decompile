using RimWorld;
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

		public override bool TryMakePreToilReservations()
		{
			IAttackTarget attackTarget = base.job.targetA.Thing as IAttackTarget;
			if (attackTarget != null)
			{
				base.pawn.Map.attackTargetReservationManager.Reserve(base.pawn, base.job, attackTarget);
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void Notify_PatherFailed()
		{
			if (base.job.attackDoorIfTargetLost)
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
					base.job.targetA = thing;
					base.job.maxNumMeleeAttacks = Rand.RangeInclusive(2, 5);
					base.job.expiryInterval = Rand.Range(2000, 4000);
					return;
				}
			}
			base.Notify_PatherFailed();
		}

		public override bool IsContinuation(Job j)
		{
			return base.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}
	}
}
