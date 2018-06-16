using System;

namespace Verse
{
	// Token: 0x02000D06 RID: 3334
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x06004988 RID: 18824 RVA: 0x00267EB8 File Offset: 0x002662B8
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x040031E0 RID: 12768
		public IntRange disappearsAfterTicks = default(IntRange);
	}
}
