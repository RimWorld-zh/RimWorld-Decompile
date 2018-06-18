using System;

namespace Verse
{
	// Token: 0x02000D1A RID: 3354
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x060049D2 RID: 18898 RVA: 0x002694F4 File Offset: 0x002678F4
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x0400320B RID: 12811
		public int healIntervalTicksStanding = 50;

		// Token: 0x0400320C RID: 12812
		public float healAmount = 1f;
	}
}
