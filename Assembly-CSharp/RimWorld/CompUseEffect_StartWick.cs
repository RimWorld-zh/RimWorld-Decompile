using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000763 RID: 1891
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x060029D1 RID: 10705 RVA: 0x001630BA File Offset: 0x001614BA
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
