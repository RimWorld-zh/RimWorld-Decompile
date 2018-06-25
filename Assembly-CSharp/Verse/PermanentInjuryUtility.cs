using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0F RID: 3343
	public static class PermanentInjuryUtility
	{
		// Token: 0x04003202 RID: 12802
		private const float PainFactorLowerGaussianWidthFactor = 1.5f;

		// Token: 0x04003203 RID: 12803
		private const float PainFactorUpperGaussianWidthFactor = 2.53f;

		// Token: 0x04003204 RID: 12804
		private const float MaxPainFactor = 12f;

		// Token: 0x060049C1 RID: 18881 RVA: 0x0026A090 File Offset: 0x00268490
		public static float GetRandomPainFactor()
		{
			float value = Rand.GaussianAsymmetric(1f, 1.5f, 2.53f);
			return Mathf.Clamp(value, 0f, 12f);
		}
	}
}
