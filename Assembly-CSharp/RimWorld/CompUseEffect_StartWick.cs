using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000765 RID: 1893
	public class CompUseEffect_StartWick : CompUseEffect
	{
		// Token: 0x060029D4 RID: 10708 RVA: 0x0016346A File Offset: 0x0016186A
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
