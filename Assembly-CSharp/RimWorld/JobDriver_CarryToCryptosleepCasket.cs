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
				return (Pawn)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Takee, base.job, 1, -1, null) && base.pawn.Reserve((Thing)this.DropPod, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_005f: stateMachine*/)._0024this.DropPod.Accepts(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_005f: stateMachine*/)._0024this.Takee)));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0085: stateMachine*/)._0024this.DropPod.GetDirectlyHeldThings().Count > 0)).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0096: stateMachine*/)._0024this.Takee.Downed)).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00a7: stateMachine*/)._0024this.pawn.CanReach((Thing)((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_00a7: stateMachine*/)._0024this.Takee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override object[] TaleParameters()
		{
			return new object[2]
			{
				base.pawn,
				this.Takee
			};
		}
	}
}
