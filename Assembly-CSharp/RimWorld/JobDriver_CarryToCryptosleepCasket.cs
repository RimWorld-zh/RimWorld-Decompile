using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		private const TargetIndex TakeeInd = TargetIndex.A;

		private const TargetIndex DropPodInd = TargetIndex.B;

		protected Pawn Takee
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator25)/*Error near IL_00aa: stateMachine*/)._003C_003Ef__this.DropPod.GetDirectlyHeldThings().Count > 0)).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator25)/*Error near IL_00bb: stateMachine*/)._003C_003Ef__this.Takee.Downed)).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator25)/*Error near IL_00cc: stateMachine*/)._003C_003Ef__this.pawn.CanReach((Thing)((_003CMakeNewToils_003Ec__Iterator25)/*Error near IL_00cc: stateMachine*/)._003C_003Ef__this.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
			Toil prepare = Toils_General.Wait(500);
			prepare.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return prepare;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator25)/*Error near IL_0181: stateMachine*/)._003C_003Ef__this.DropPod.TryAcceptThing(((_003CMakeNewToils_003Ec__Iterator25)/*Error near IL_0181: stateMachine*/)._003C_003Ef__this.Takee, true);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
