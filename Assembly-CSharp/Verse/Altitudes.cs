using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFD RID: 3069
	public static class Altitudes
	{
		// Token: 0x04002DE3 RID: 11747
		private const int NumAltitudeLayers = 31;

		// Token: 0x04002DE4 RID: 11748
		private static readonly float[] Alts = new float[31];

		// Token: 0x04002DE5 RID: 11749
		private const float LayerSpacing = 0.46875f;

		// Token: 0x04002DE6 RID: 11750
		public const float AltInc = 0.046875f;

		// Token: 0x04002DE7 RID: 11751
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.046875f, 0f);

		// Token: 0x06004300 RID: 17152 RVA: 0x0023837C File Offset: 0x0023677C
		static Altitudes()
		{
			for (int i = 0; i < 31; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.46875f;
			}
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x002383D4 File Offset: 0x002367D4
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}
	}
}
