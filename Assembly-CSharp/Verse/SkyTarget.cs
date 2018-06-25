using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB0 RID: 3248
	public struct SkyTarget
	{
		// Token: 0x04003096 RID: 12438
		public float glow;

		// Token: 0x04003097 RID: 12439
		public SkyColorSet colors;

		// Token: 0x04003098 RID: 12440
		public float lightsourceShineSize;

		// Token: 0x04003099 RID: 12441
		public float lightsourceShineIntensity;

		// Token: 0x0600479A RID: 18330 RVA: 0x0025C6E2 File Offset: 0x0025AAE2
		public SkyTarget(float glow, SkyColorSet colorSet, float lightsourceShineSize, float lightsourceShineIntensity)
		{
			this.glow = glow;
			this.lightsourceShineSize = lightsourceShineSize;
			this.lightsourceShineIntensity = lightsourceShineIntensity;
			this.colors = colorSet;
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0025C704 File Offset: 0x0025AB04
		public static SkyTarget Lerp(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, B.glow, t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, B.lightsourceShineSize, t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, B.lightsourceShineIntensity, t)
			};
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x0025C790 File Offset: 0x0025AB90
		public static SkyTarget LerpDarken(SkyTarget A, SkyTarget B, float t)
		{
			return new SkyTarget
			{
				colors = SkyColorSet.Lerp(A.colors, B.colors, t),
				glow = Mathf.Lerp(A.glow, Mathf.Min(A.glow, B.glow), t),
				lightsourceShineSize = Mathf.Lerp(A.lightsourceShineSize, Mathf.Min(A.lightsourceShineSize, B.lightsourceShineSize), t),
				lightsourceShineIntensity = Mathf.Lerp(A.lightsourceShineIntensity, Mathf.Min(A.lightsourceShineIntensity, B.lightsourceShineIntensity), t)
			};
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0025C840 File Offset: 0x0025AC40
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(glow=",
				this.glow.ToString("F2"),
				", colors=",
				this.colors.ToString(),
				", lightsourceShineSize=",
				this.lightsourceShineSize.ToString(),
				", lightsourceShineIntensity=",
				this.lightsourceShineIntensity.ToString(),
				")"
			});
		}
	}
}
