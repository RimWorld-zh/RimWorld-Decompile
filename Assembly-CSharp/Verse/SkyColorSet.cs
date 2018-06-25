using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B2A RID: 2858
	public struct SkyColorSet
	{
		// Token: 0x040028C7 RID: 10439
		public Color sky;

		// Token: 0x040028C8 RID: 10440
		public Color shadow;

		// Token: 0x040028C9 RID: 10441
		public Color overlay;

		// Token: 0x040028CA RID: 10442
		public float saturation;

		// Token: 0x06003F00 RID: 16128 RVA: 0x0021312C File Offset: 0x0021152C
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0021314C File Offset: 0x0021154C
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

		// Token: 0x06003F02 RID: 16130 RVA: 0x002131D8 File Offset: 0x002115D8
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
