using System;

namespace Verse
{
	// Token: 0x02000D18 RID: 3352
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x04003218 RID: 12824
		public int ticksSinceHeal = 0;

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x0026A964 File Offset: 0x00268D64
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x0026A984 File Offset: 0x00268D84
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x0026A999 File Offset: 0x00268D99
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}
	}
}
