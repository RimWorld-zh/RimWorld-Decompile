using System;

namespace Verse
{
	// Token: 0x02000D16 RID: 3350
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x04003214 RID: 12820
		public float infectionChance = 0.5f;

		// Token: 0x060049D8 RID: 18904 RVA: 0x0026A82A File Offset: 0x00268C2A
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}
	}
}
