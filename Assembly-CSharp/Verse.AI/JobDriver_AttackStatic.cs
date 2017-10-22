using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_AttackStatic : JobDriver
	{
		private bool startedIncapacitated;

		private int numAttacksMade;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn pawn2 = ((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.TargetThingA as Pawn;
					if (pawn2 != null)
					{
						((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.startedIncapacitated = pawn2.Downed;
					}
					((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn.pather.StopDead();
				},
				tickAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.TargetA.HasThing)
					{
						Pawn pawn = ((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.TargetA.Thing as Pawn;
						if (!((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.TargetA.Thing.Destroyed && (pawn == null || ((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.startedIncapacitated || !pawn.Downed))
						{
							goto IL_007c;
						}
						((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
						return;
					}
					goto IL_007c;
					IL_007c:
					if (((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.numAttacksMade >= ((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.CurJob.maxNumStaticAttacks && !((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.stances.FullBodyBusy)
					{
						((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Succeeded);
					}
					else if (((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.equipment.TryStartAttack(((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.TargetA))
					{
						((_003CMakeNewToils_003Ec__Iterator1B6)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.numAttacksMade++;
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}
	}
}
