using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D0F RID: 3343
	public static class PermanentInjuryUtility
	{
		// Token: 0x060049AD RID: 18861 RVA: 0x002688A0 File Offset: 0x00266CA0
		public static float GetRandomPainFactor()
		{
			float value = Rand.GaussianAsymmetric(1f, 1.5f, 2.53f);
			return Mathf.Clamp(value, 0f, 12f);
		}

		// Token: 0x040031F0 RID: 12784
		private const float PainFactorLowerGaussianWidthFactor = 1.5f;

		// Token: 0x040031F1 RID: 12785
		private const float PainFactorUpperGaussianWidthFactor = 2.53f;

		// Token: 0x040031F2 RID: 12786
		private const float MaxPainFactor = 12f;
	}
}
