using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Slaughter : JobDriver
	{
		private const int SlaughterDuration = 180;

		protected Pawn Victim
		{
			get
			{
				return (Pawn)base.CurJob.targetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Slaughter);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					ExecutionUtility.DoExecutionByCut(((_003CMakeNewToils_003Ec__Iterator7)/*Error near IL_00b0: stateMachine*/)._003Cexecute_003E__0.actor, ((_003CMakeNewToils_003Ec__Iterator7)/*Error near IL_00b0: stateMachine*/)._003C_003Ef__this.Victim);
					((_003CMakeNewToils_003Ec__Iterator7)/*Error near IL_00b0: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
