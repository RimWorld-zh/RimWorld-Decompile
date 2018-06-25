using System;

namespace Verse
{
	// Token: 0x02000D1A RID: 3354
	public class HediffComp_SelfHeal : HediffComp
	{
		// Token: 0x04003218 RID: 12824
		public int ticksSinceHeal = 0;

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x0026AA40 File Offset: 0x00268E40
		public HediffCompProperties_SelfHeal Props
		{
			get
			{
				return (HediffCompProperties_SelfHeal)this.props;
			}
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x0026AA60 File Offset: 0x00268E60
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceHeal, "ticksSinceHeal", 0, false);
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x0026AA75 File Offset: 0x00268E75
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
