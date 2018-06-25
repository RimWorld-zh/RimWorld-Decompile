using System;

namespace Verse
{
	// Token: 0x02000B10 RID: 2832
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x040027EF RID: 10223
		public float glow = 1f;

		// Token: 0x040027F0 RID: 10224
		public SkyColorSet skyColors;

		// Token: 0x040027F1 RID: 10225
		public float lightsourceShineSize = 1f;

		// Token: 0x040027F2 RID: 10226
		public float lightsourceShineIntensity = 1f;

		// Token: 0x040027F3 RID: 10227
		public bool lerpDarken;

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0020FCAE File Offset: 0x0020E0AE
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}
	}
}
