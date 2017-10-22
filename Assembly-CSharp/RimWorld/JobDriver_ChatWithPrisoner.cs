using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		protected Pawn Talkee
		{
			get
			{
				return (Pawn)base.CurJob.targetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator49)/*Error near IL_0086: stateMachine*/)._003C_003Ef__this.Talkee.IsPrisonerOfColony || !((_003CMakeNewToils_003Ec__Iterator49)/*Error near IL_0086: stateMachine*/)._003C_003Ef__this.Talkee.guest.PrisonerIsSecure));
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(base.pawn);
			yield return Toils_Interpersonal.ConvinceRecruitee(base.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.ConvinceRecruitee(base.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.ConvinceRecruitee(base.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.ConvinceRecruitee(base.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.ConvinceRecruitee(base.pawn, this.Talkee);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			if (base.CurJob.def == JobDefOf.PrisonerAttemptRecruit)
			{
				yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			}
		}
	}
}
