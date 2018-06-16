using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200002F RID: 47
	public class JobDriver_Nuzzle : JobDriver
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00012580 File Offset: 0x00010980
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00012598 File Offset: 0x00010998
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil gotoTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			gotoTarget.socialMode = RandomSocialMode.Off;
			Toil wait = Toils_General.WaitWith(TargetIndex.A, 100, false, true);
			wait.socialMode = RandomSocialMode.Off;
			yield return Toils_General.Do(delegate
			{
				Pawn recipient = (Pawn)this.pawn.CurJob.targetA.Thing;
				this.pawn.interactions.TryInteractWith(recipient, InteractionDefOf.Nuzzle);
			});
			yield break;
		}

		// Token: 0x040001B0 RID: 432
		private const int NuzzleDuration = 100;
	}
}
