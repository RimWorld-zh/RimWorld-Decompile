using System;

namespace Verse
{
	// Token: 0x02000D04 RID: 3332
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x040031E9 RID: 12777
		public IntRange disappearsAfterTicks = default(IntRange);

		// Token: 0x0600499A RID: 18842 RVA: 0x00269384 File Offset: 0x00267784
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}
	}
}
