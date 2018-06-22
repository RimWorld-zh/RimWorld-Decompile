using System;

namespace Verse
{
	// Token: 0x02000D02 RID: 3330
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x06004997 RID: 18839 RVA: 0x002692A8 File Offset: 0x002676A8
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x040031E9 RID: 12777
		public IntRange disappearsAfterTicks = default(IntRange);
	}
}
