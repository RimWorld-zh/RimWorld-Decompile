using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B2C RID: 2860
	public struct SkyColorSet
	{
		// Token: 0x06003F00 RID: 16128 RVA: 0x00212CC4 File Offset: 0x002110C4
		public SkyColorSet(Color sky, Color shadow, Color overlay, float saturation)
		{
			this.sky = sky;
			this.shadow = shadow;
			this.overlay = overlay;
			this.saturation = saturation;
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x00212CE4 File Offset: 0x002110E4
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

		// Token: 0x06003F02 RID: 16130 RVA: 0x00212D70 File Offset: 0x00211170
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

		// Token: 0x040028CA RID: 10442
		public Color sky;

		// Token: 0x040028CB RID: 10443
		public Color shadow;

		// Token: 0x040028CC RID: 10444
		public Color overlay;

		// Token: 0x040028CD RID: 10445
		public float saturation;
	}
}
