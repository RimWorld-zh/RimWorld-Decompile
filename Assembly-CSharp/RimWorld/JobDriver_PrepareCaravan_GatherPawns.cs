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
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__IteratorA)/*Error near IL_002f: stateMachine*/)._003C_003Ef__this.Map.lordManager.lords.Contains(((_003CMakeNewToils_003Ec__IteratorA)/*Error near IL_002f: stateMachine*/)._003C_003Ef__this.CurJob.lord)));
			this.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__IteratorA)/*Error near IL_0047: stateMachine*/)._003C_003Ef__this.AnimalOrSlave.GetLord() != ((_003CMakeNewToils_003Ec__IteratorA)/*Error near IL_0047: stateMachine*/)._003C_003Ef__this.CurJob.lord));
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn((Func<bool>)(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(((_003CMakeNewToils_003Ec__IteratorA)/*Error near IL_0082: stateMachine*/)._003C_003Ef__this.AnimalOrSlave)));
			yield return this.SetFollowerToil();
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
