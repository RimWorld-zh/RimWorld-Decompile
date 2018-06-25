using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B2B RID: 2859
	public struct SkyColorSet
	{
		// Token: 0x040028CE RID: 10446
		public Color sky;

		// Token: 0x040028CF RID: 10447
		public Color shadow;

		// Token: 0x040028D0 RID: 10448
		public Color overlay;

		// Token: 0x040028D1 RID: 10449
		public float saturation;

		// Token: 0x06003F00 RID: 16128 RVA: 0x0021340C File Offset: 0x0021180C
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0021342C File Offset: 0x0021182C
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

		// Token: 0x06003F02 RID: 16130 RVA: 0x002134B8 File Offset: 0x002118B8
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
