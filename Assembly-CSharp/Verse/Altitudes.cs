using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFA RID: 3066
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

		// Token: 0x060042FD RID: 17149 RVA: 0x00237FC0 File Offset: 0x002363C0
		static Altitudes()
		{
			for (int i = 0; i < 31; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.46875f;
			}
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00238018 File Offset: 0x00236418
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}
	}
}
