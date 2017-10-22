using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Execute : JobDriver
	{
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
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.Victim.IsPrisonerOfColony || !((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.Victim.guest.PrisonerIsSecure));
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					ExecutionUtility.DoExecutionByCut(((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_00a5: stateMachine*/)._003Cexecute_003E__0.actor, ((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_00a5: stateMachine*/)._003C_003Ef__this.Victim);
					ThoughtUtility.GiveThoughtsForPawnExecuted(((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_00a5: stateMachine*/)._003C_003Ef__this.Victim, PawnExecutionKind.GenericBrutal);
					TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, ((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_00a5: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator4A)/*Error near IL_00a5: stateMachine*/)._003C_003Ef__this.Victim);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
