using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200007E RID: 126
	public class JobDriver_SingleInteraction : JobDriver
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00025248 File Offset: 0x00023648
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00025278 File Offset: 0x00023678
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00025290 File Offset: 0x00023690
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil finalGoto = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			finalGoto.socialMode = RandomSocialMode.Off;
			yield return finalGoto;
			yield return Toils_Interpersonal.Interact(TargetIndex.A, this.job.interaction);
			yield break;
		}

		// Token: 0x04000236 RID: 566
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
