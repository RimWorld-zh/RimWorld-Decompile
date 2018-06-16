using System;

namespace Verse
{
	// Token: 0x02000D0E RID: 3342
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x060049A6 RID: 18854 RVA: 0x0026864D File Offset: 0x00266A4D
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x040031EC RID: 12780
		public float becomePermanentChance = 1f;

		// Token: 0x040031ED RID: 12781
		public string permanentLabel = null;

		// Token: 0x040031EE RID: 12782
		public string instantlyPermanentLabel = null;
	}
}
