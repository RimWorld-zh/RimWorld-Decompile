using System;

namespace Verse
{
	// Token: 0x02000D05 RID: 3333
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x040031EA RID: 12778
		private int ticksToDisappear = 0;

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600499C RID: 18844 RVA: 0x002693C8 File Offset: 0x002677C8
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600499D RID: 18845 RVA: 0x002693E8 File Offset: 0x002677E8
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0;
			}
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00269417 File Offset: 0x00267817
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00269436 File Offset: 0x00267836
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00269448 File Offset: 0x00267848
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00269487 File Offset: 0x00267887
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x0026949C File Offset: 0x0026789C
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}
	}
}
