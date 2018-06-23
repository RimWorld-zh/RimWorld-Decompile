using System;

namespace Verse
{
	// Token: 0x02000D13 RID: 3347
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x0400320D RID: 12813
		public float infectionChance = 0.5f;

		// Token: 0x060049D5 RID: 18901 RVA: 0x0026A46E File Offset: 0x0026886E
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}
	}
}
