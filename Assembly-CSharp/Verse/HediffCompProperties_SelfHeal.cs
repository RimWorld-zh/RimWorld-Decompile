using System;

namespace Verse
{
	// Token: 0x02000D17 RID: 3351
	public class HediffCompProperties_SelfHeal : HediffCompProperties
	{
		// Token: 0x060049E3 RID: 18915 RVA: 0x0026A928 File Offset: 0x00268D28
		public HediffCompProperties_SelfHeal()
		{
			this.compClass = typeof(HediffComp_SelfHeal);
		}

		// Token: 0x04003216 RID: 12822
		public int healIntervalTicksStanding = 50;

		// Token: 0x04003217 RID: 12823
		public float healAmount = 1f;
	}
}
