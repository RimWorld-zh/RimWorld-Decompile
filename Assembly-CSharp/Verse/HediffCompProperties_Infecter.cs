using System;

namespace Verse
{
	// Token: 0x02000D16 RID: 3350
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x060049C4 RID: 18884 RVA: 0x0026903A File Offset: 0x0026743A
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x04003202 RID: 12802
		public float infectionChance = 0.5f;
	}
}
