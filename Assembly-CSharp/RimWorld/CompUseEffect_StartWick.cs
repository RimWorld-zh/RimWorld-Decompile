using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000767 RID: 1895
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x060029D6 RID: 10710 RVA: 0x00162E4E File Offset: 0x0016124E
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
