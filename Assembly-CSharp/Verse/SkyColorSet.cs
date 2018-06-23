using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B28 RID: 2856
	public struct SkyColorSet
	{
		// Token: 0x040028C6 RID: 10438
		public Color sky;

		// Token: 0x040028C7 RID: 10439
		public Color shadow;

		// Token: 0x040028C8 RID: 10440
		public Color overlay;

		// Token: 0x040028C9 RID: 10441
		public float saturation;

		// Token: 0x06003EFC RID: 16124 RVA: 0x00213000 File Offset: 0x00211400
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00213020 File Offset: 0x00211420
		public static SkyColorSet Lerp(SkyColorSet A, SkyColorSet B, float t)
		{
			return new SkyColorSet
			{
				sky = Color.Lerp(A.sky, B.sky, t),
				shadow = Color.Lerp(A.shadow, B.shadow, t),
				overlay = Color.Lerp(A.overlay, B.overlay, t),
				saturation = Mathf.Lerp(A.saturation, B.saturation, t)
			};
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x002130AC File Offset: 0x002114AC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(sky=",
				this.sky,
				", shadow=",
				this.shadow,
				", overlay=",
				this.overlay,
				", sat=",
				this.saturation,
				")"
			});
		}
	}
}
