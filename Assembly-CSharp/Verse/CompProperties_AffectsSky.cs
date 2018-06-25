using System;

namespace Verse
{
	// Token: 0x02000B0F RID: 2831
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x040027E8 RID: 10216
		public float glow = 1f;

		// Token: 0x040027E9 RID: 10217
		public SkyColorSet skyColors;

		// Token: 0x040027EA RID: 10218
		public float lightsourceShineSize = 1f;

		// Token: 0x040027EB RID: 10219
		public float lightsourceShineIntensity = 1f;

		// Token: 0x040027EC RID: 10220
		public bool lerpDarken;

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0020F9CE File Offset: 0x0020DDCE
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}
	}
}
