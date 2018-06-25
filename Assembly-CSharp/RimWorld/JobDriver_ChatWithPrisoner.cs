using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000091 RID: 145
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x000291E4 File Offset: 0x000275E4
		protected Pawn Talkee
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00029210 File Offset: 0x00027610
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00029244 File Offset: 0x00027644
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Talkee.IsPrisonerOfColony || !this.Talkee.guest.PrisonerIsSecure);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			if (this.job.def == JobDefOf.PrisonerAttemptRecruit)
			{
				yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			}
			yield break;
		}
	}
}
