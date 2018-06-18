using System;

namespace Verse
{
	// Token: 0x02000D06 RID: 3334
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06004988 RID: 18824 RVA: 0x00267ED4 File Offset: 0x002662D4
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x00267EF4 File Offset: 0x002662F4
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0;
			}
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00267F23 File Offset: 0x00266323
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00267F42 File Offset: 0x00266342
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00267F54 File Offset: 0x00266354
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00267F93 File Offset: 0x00266393
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00267FA8 File Offset: 0x002663A8
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x040031DF RID: 12767
		private int ticksToDisappear = 0;
	}
}
