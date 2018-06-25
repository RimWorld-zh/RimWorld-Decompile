using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB1 RID: 3249
	public struct SkyTarget
	{
		// Token: 0x0400309D RID: 12445
		public float glow;

		// Token: 0x0400309E RID: 12446
		public SkyColorSet colors;

		// Token: 0x0400309F RID: 12447
		public float lightsourceShineSize;

		// Token: 0x040030A0 RID: 12448
		public float lightsourceShineIntensity;

		// Token: 0x0600479A RID: 18330 RVA: 0x0025C9C2 File Offset: 0x0025ADC2
		public SkyTarget(float glow, SkyColorSet colorSet, float lightsourceShineSize, float lightsourceShineIntensity)
		{
			this.glow = glow;
			this.lightsourceShineSize = lightsourceShineSize;
			this.lightsourceShineIntensity = lightsourceShineIntensity;
			this.colors = colorSet;
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x0025C9E4 File Offset: 0x0025ADE4
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

		// Token: 0x0600479C RID: 18332 RVA: 0x0025CA70 File Offset: 0x0025AE70
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

		// Token: 0x0600479D RID: 18333 RVA: 0x0025CB20 File Offset: 0x0025AF20
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
