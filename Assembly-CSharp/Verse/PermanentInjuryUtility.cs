using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0E RID: 3342
	public static class PermanentInjuryUtility
	{
		// Token: 0x040031FB RID: 12795
		private const float PainFactorLowerGaussianWidthFactor = 1.5f;

		// Token: 0x040031FC RID: 12796
		private const float PainFactorUpperGaussianWidthFactor = 2.53f;

		// Token: 0x040031FD RID: 12797
		private const float MaxPainFactor = 12f;

		// Token: 0x060049C1 RID: 18881 RVA: 0x00269DB0 File Offset: 0x002681B0
		public static float GetRandomPainFactor()
		{
			float value = Rand.GaussianAsymmetric(1f, 1.5f, 2.53f);
			return Mathf.Clamp(value, 0f, 12f);
		}
	}
}
