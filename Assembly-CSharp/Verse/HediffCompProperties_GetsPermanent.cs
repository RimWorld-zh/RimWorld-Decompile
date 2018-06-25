using System;

namespace Verse
{
	// Token: 0x02000D0D RID: 3341
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x040031FC RID: 12796
		public float becomePermanentChance = 1f;

		// Token: 0x040031FD RID: 12797
		public string permanentLabel = null;

		// Token: 0x040031FE RID: 12798
		public string instantlyPermanentLabel = null;

		// Token: 0x060049B8 RID: 18872 RVA: 0x00269E15 File Offset: 0x00268215
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}
	}
}
