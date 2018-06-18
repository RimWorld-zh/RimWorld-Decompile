using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFD RID: 3069
	public static class Altitudes
	{
		// Token: 0x060042F4 RID: 17140 RVA: 0x00236C30 File Offset: 0x00235030
		static Altitudes()
		{
			for (int i = 0; i < 31; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.46875f;
			}
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00236C88 File Offset: 0x00235088
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x04002DD2 RID: 11730
		private const int NumAltitudeLayers = 31;

		// Token: 0x04002DD3 RID: 11731
		private static readonly float[] Alts = new float[31];

		// Token: 0x04002DD4 RID: 11732
		private const float LayerSpacing = 0.46875f;

		// Token: 0x04002DD5 RID: 11733
		public const float AltInc = 0.046875f;

		// Token: 0x04002DD6 RID: 11734
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.046875f, 0f);
	}
}
