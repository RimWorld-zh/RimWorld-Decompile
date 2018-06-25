using System;

namespace Verse
{
	// Token: 0x02000D1A RID: 3354
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x0400321D RID: 12829
		public int healIntervalTicksStanding = 50;

		// Token: 0x0400321E RID: 12830
		public float healAmount = 1f;

		// Token: 0x060049E6 RID: 18918 RVA: 0x0026ACE4 File Offset: 0x002690E4
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}
	}
}
