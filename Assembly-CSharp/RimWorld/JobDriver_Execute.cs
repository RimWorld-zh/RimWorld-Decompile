using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000092 RID: 146
	public class JobDriver_Execute : JobDriver
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00029824 File Offset: 0x00027C24
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00029850 File Offset: 0x00027C50
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00029884 File Offset: 0x00027C84
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !this.Victim.IsPrisonerOfColony || !this.Victim.guest.PrisonerIsSecure);
			Toil execute = new Toil();
			execute.initAction = delegate()
			{
				ExecutionUtility.DoExecutionByCut(execute.actor, this.Victim);
				ThoughtUtility.GiveThoughtsForPawnExecuted(this.Victim, PawnExecutionKind.GenericBrutal);
				TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
				{
					this.pawn,
					this.Victim
				});
			};
			execute.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return execute;
			yield break;
		}
	}
}
