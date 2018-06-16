using System;

namespace Verse
{
	// Token: 0x02000D07 RID: 3335
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x00267EFC File Offset: 0x002662FC
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600498B RID: 18827 RVA: 0x00267F1C File Offset: 0x0026631C
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0;
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00267F4B File Offset: 0x0026634B
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ticksToDisappear = this.Props.disappearsAfterTicks.RandomInRange;
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00267F6A File Offset: 0x0026636A
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear--;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00267F7C File Offset: 0x0026637C
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00267FBB File Offset: 0x002663BB
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00267FD0 File Offset: 0x002663D0
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear;
		}

		// Token: 0x040031E1 RID: 12769
		private int ticksToDisappear = 0;
	}
}
