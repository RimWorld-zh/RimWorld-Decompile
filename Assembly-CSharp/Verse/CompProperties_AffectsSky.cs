using System;

namespace Verse
{
	// Token: 0x02000B0D RID: 2829
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x06003E9D RID: 16029 RVA: 0x0020F8A2 File Offset: 0x0020DCA2
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x040027E7 RID: 10215
		public float glow = 1f;

		// Token: 0x040027E8 RID: 10216
		public SkyColorSet skyColors;

		// Token: 0x040027E9 RID: 10217
		public float lightsourceShineSize = 1f;

		// Token: 0x040027EA RID: 10218
		public float lightsourceShineIntensity = 1f;

		// Token: 0x040027EB RID: 10219
		public bool lerpDarken;
	}
}
