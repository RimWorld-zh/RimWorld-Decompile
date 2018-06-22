using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075F RID: 1887
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x001621E8 File Offset: 0x001605E8
		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x00162202 File Offset: 0x00160602
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
