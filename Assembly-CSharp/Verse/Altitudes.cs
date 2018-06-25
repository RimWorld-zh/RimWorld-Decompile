using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFC RID: 3068
	public static class Altitudes
	{
		// Token: 0x04002DDC RID: 11740
		private const int NumAltitudeLayers = 31;

		// Token: 0x04002DDD RID: 11741
		private static readonly float[] Alts = new float[31];

		// Token: 0x04002DDE RID: 11742
		private const float LayerSpacing = 0.46875f;

		// Token: 0x04002DDF RID: 11743
		public const float AltInc = 0.046875f;

		// Token: 0x04002DE0 RID: 11744
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.046875f, 0f);

		// Token: 0x06004300 RID: 17152 RVA: 0x0023809C File Offset: 0x0023649C
		static Altitudes()
		{
			for (int i = 0; i < 31; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.46875f;
			}
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x002380F4 File Offset: 0x002364F4
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}
	}
}
