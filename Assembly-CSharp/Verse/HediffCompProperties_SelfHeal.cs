using System;

namespace Verse
{
	// Token: 0x02000D1B RID: 3355
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x060049D4 RID: 18900 RVA: 0x0026951C File Offset: 0x0026791C
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x0400320D RID: 12813
		public int healIntervalTicksStanding = 50;

		// Token: 0x0400320E RID: 12814
		public float healAmount = 1f;
	}
}
