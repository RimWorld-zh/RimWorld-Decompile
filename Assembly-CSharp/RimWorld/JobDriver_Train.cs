using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000037 RID: 55
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		// Token: 0x060001DE RID: 478 RVA: 0x000146BC File Offset: 0x00012ABC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return toil;
			}
			this.FailOn(() => base.Animal.training.NextTrainableToTrain() == null && !base.OnLastToil);
			yield break;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000146E8 File Offset: 0x00012AE8
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryTrain(TargetIndex.A);
		}
	}
}
