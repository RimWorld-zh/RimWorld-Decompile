using System;

namespace Verse
{
	// Token: 0x02000D05 RID: 3333
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x06004986 RID: 18822 RVA: 0x00267E90 File Offset: 0x00266290
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x040031DE RID: 12766
		public IntRange disappearsAfterTicks = default(IntRange);
	}
}
