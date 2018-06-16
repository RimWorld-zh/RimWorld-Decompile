using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFE RID: 3070
	public static class Altitudes
	{
		// Token: 0x060042F6 RID: 17142 RVA: 0x00236C58 File Offset: 0x00235058
		static Altitudes()
		{
			for (int i = 0; i < 31; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.46875f;
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00236CB0 File Offset: 0x002350B0
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x04002DD4 RID: 11732
		private const int NumAltitudeLayers = 31;

		// Token: 0x04002DD5 RID: 11733
		private static readonly float[] Alts = new float[31];

		// Token: 0x04002DD6 RID: 11734
		private const float LayerSpacing = 0.46875f;

		// Token: 0x04002DD7 RID: 11735
		public const float AltInc = 0.046875f;

		// Token: 0x04002DD8 RID: 11736
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.046875f, 0f);
	}
}
