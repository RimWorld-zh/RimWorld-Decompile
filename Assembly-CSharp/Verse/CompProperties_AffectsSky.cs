using System;

namespace Verse
{
	// Token: 0x02000B11 RID: 2833
	public class CompProperties_AffectsSky : CompProperties
	{
		// Token: 0x06003E9F RID: 16031 RVA: 0x0020F492 File Offset: 0x0020D892
		public CompProperties_AffectsSky()
		{
			this.compClass = typeof(CompAffectsSky);
		}

		// Token: 0x040027EB RID: 10219
		public float glow = 1f;

		// Token: 0x040027EC RID: 10220
		public SkyColorSet skyColors;

		// Token: 0x040027ED RID: 10221
		public float lightsourceShineSize = 1f;

		// Token: 0x040027EE RID: 10222
		public float lightsourceShineIntensity = 1f;

		// Token: 0x040027EF RID: 10223
		public bool lerpDarken;
	}
}
