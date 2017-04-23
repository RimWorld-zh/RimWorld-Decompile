using System;
using System.Collections.Generic;
using System.Diagnostics;

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

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AttackMelee.<MakeNewToils>c__Iterator1AD <MakeNewToils>c__Iterator1AD = new JobDriver_AttackMelee.<MakeNewToils>c__Iterator1AD();
			<MakeNewToils>c__Iterator1AD.<>f__this = this;
			JobDriver_AttackMelee.<MakeNewToils>c__Iterator1AD expr_0E = <MakeNewToils>c__Iterator1AD;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void Notify_PatherFailed()
		{
			if (base.CurJob.attackDoorIfTargetLost)
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
