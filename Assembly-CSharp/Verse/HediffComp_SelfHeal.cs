using System;

namespace Verse
{
	// Token: 0x02000D1B RID: 3355
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060049D4 RID: 18900 RVA: 0x00269530 File Offset: 0x00267930
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x00269550 File Offset: 0x00267950
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x00269565 File Offset: 0x00267965
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x0400320D RID: 12813
		public int ticksSinceHeal = 0;
	}
}
