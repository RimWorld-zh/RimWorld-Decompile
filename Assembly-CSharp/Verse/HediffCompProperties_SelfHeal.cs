using System;

namespace Verse
{
	// Token: 0x02000D19 RID: 3353
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x04003216 RID: 12822
		public int healIntervalTicksStanding = 50;

		// Token: 0x04003217 RID: 12823
		public float healAmount = 1f;

		// Token: 0x060049E6 RID: 18918 RVA: 0x0026AA04 File Offset: 0x00268E04
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}
	}
}
