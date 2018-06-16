using System;

namespace Verse
{
	// Token: 0x02000D1C RID: 3356
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060049D6 RID: 18902 RVA: 0x00269558 File Offset: 0x00267958
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x00269578 File Offset: 0x00267978
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x0026958D File Offset: 0x0026798D
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksSinceHeal++;
			if (this.ticksSinceHeal > this.Props.healIntervalTicksStanding)
			{
				severityAdjustment -= this.Props.healAmount;
				this.ticksSinceHeal = 0;
			}
		}

		// Token: 0x0400320F RID: 12815
		public int ticksSinceHeal = 0;
	}
}
