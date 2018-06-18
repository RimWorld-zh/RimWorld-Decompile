using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000763 RID: 1891
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060029BE RID: 10686 RVA: 0x00162010 File Offset: 0x00160410
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x0016202A File Offset: 0x0016042A
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
