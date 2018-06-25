using System;

namespace Verse
{
	// Token: 0x02000D1B RID: 3355
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x0400321F RID: 12831
		public int ticksSinceHeal = 0;

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x0026AD20 File Offset: 0x00269120
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x0026AD40 File Offset: 0x00269140
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x0026AD55 File Offset: 0x00269155
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
