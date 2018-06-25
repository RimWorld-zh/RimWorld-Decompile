using System;

namespace Verse
{
	// Token: 0x02000D05 RID: 3333
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x040031F0 RID: 12784
		public IntRange disappearsAfterTicks = default(IntRange);

		// Token: 0x0600499A RID: 18842 RVA: 0x00269664 File Offset: 0x00267A64
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}
	}
}
