using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000761 RID: 1889
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x00162338 File Offset: 0x00160738
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x00162352 File Offset: 0x00160752
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
