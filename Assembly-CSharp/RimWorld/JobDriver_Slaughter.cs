using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000035 RID: 53
	public class JobDriver_Slaughter : JobDriver
	{
		// Token: 0x040001C1 RID: 449
		public const int SlaughterDuration = 180;

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00014198 File Offset: 0x00012598
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000141C4 File Offset: 0x000125C4
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000141F8 File Offset: 0x000125F8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Slaughter);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false);
			yield return Toils_General.Do(delegate
			{
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			});
			yield break;
		}
	}
}
