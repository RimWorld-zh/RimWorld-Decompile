using System;

namespace Verse
{
	// Token: 0x02000D0A RID: 3338
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x060049B5 RID: 18869 RVA: 0x00269A59 File Offset: 0x00267E59
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x040031F5 RID: 12789
		public float becomePermanentChance = 1f;

		// Token: 0x040031F6 RID: 12790
		public string permanentLabel = null;

		// Token: 0x040031F7 RID: 12791
		public string instantlyPermanentLabel = null;
	}
}
