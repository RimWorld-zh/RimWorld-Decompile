using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000767 RID: 1895
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x060029D8 RID: 10712 RVA: 0x00162EE2 File Offset: 0x001612E2
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
