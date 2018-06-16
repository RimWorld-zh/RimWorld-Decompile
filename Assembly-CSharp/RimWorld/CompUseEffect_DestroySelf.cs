using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000763 RID: 1891
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x00161F7C File Offset: 0x0016037C
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x00161F96 File Offset: 0x00160396
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
