using System;

namespace Verse
{
	// Token: 0x02000D03 RID: 3331
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004999 RID: 18841 RVA: 0x002692EC File Offset: 0x002676EC
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600499A RID: 18842 RVA: 0x0026930C File Offset: 0x0026770C
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0;
			}
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x0026933B File Offset: 0x0026773B
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x0026935A File Offset: 0x0026775A
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x0026936C File Offset: 0x0026776C
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x002693AB File Offset: 0x002677AB
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x002693C0 File Offset: 0x002677C0
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x040031EA RID: 12778
		private int ticksToDisappear = 0;
	}
}
