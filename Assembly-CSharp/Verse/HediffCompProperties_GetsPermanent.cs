using System;

namespace Verse
{
	// Token: 0x02000D0D RID: 3341
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x060049A4 RID: 18852 RVA: 0x00268625 File Offset: 0x00266A25
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x040031EA RID: 12778
		public float becomePermanentChance = 1f;

		// Token: 0x040031EB RID: 12779
		public string permanentLabel = null;

		// Token: 0x040031EC RID: 12780
		public string instantlyPermanentLabel = null;
	}
}
