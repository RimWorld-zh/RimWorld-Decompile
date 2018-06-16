using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000070 RID: 112
	public class JobDriver_Insult : JobDriver
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00021310 File Offset: 0x0001F710
		private Pawn Target
		{
			get
			{
				return (Pawn)((Thing)this.pawn.CurJob.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00021340 File Offset: 0x0001F740
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00021358 File Offset: 0x0001F758
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return this.InsultingSpreeDelayToil();
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			Toil finalGoto = Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			finalGoto.socialMode = RandomSocialMode.Off;
			yield return finalGoto;
			yield return this.InteractToil();
			yield break;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00021384 File Offset: 0x0001F784
		private Toil InteractToil()
		{
			return Toils_General.Do(delegate
			{
				if (this.pawn.interactions.TryInteractWith(this.Target, InteractionDefOf.Insult))
				{
					MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
					if (mentalState_InsultingSpree != null)
					{
						mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
						if (mentalState_InsultingSpree.target == this.Target)
						{
							mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
						}
					}
				}
			});
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000213AC File Offset: 0x0001F7AC
		private Toil InsultingSpreeDelayToil()
		{
			Action action = delegate()
			{
				MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
				if (mentalState_InsultingSpree == null || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			return new Toil
			{
				initAction = action,
				tickAction = action,
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Never
			};
		}

		// Token: 0x04000218 RID: 536
		private const TargetIndex TargetInd = TargetIndex.A;
	}
}
