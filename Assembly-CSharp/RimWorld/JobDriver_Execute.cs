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
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00029848 File Offset: 0x00027C48
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00029874 File Offset: 0x00027C74
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000298A8 File Offset: 0x00027CA8
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
