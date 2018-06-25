using System;

namespace Verse
{
	// Token: 0x02000D15 RID: 3349
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x0400320D RID: 12813
		public float infectionChance = 0.5f;

		// Token: 0x060049D8 RID: 18904 RVA: 0x0026A54A File Offset: 0x0026894A
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}
	}
}
