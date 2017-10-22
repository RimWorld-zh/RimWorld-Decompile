using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;

		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.AnimalOrSlave, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_002c: stateMachine*/)._0024this.Map.lordManager.lords.Contains(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_002c: stateMachine*/)._0024this.job.lord)));
			this.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.AnimalOrSlave == null || ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.AnimalOrSlave.GetLord() != ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.job.lord));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn((Func<bool>)(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0064: stateMachine*/)._0024this.AnimalOrSlave)));
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private Toil SetFollowerToil()
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate
			{
				GatherAnimalsAndSlavesForCaravanUtility.SetFollower(this.AnimalOrSlave, base.pawn);
				RestUtility.WakeUp(base.pawn);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}
	}
}
