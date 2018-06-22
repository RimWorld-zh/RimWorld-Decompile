using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000036 RID: 54
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00014418 File Offset: 0x00012818
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return toil;
			}
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
			yield break;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00014444 File Offset: 0x00012844
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryRecruit(TargetIndex.A);
		}
	}
}
