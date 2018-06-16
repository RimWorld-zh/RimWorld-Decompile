using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D10 RID: 3344
	public static class PermanentInjuryUtility
	{
		// Token: 0x060049AF RID: 18863 RVA: 0x002688C8 File Offset: 0x00266CC8
		public static float GetRandomPainFactor()
		{
			float value = Rand.GaussianAsymmetric(1f, 1.5f, 2.53f);
			return Mathf.Clamp(value, 0f, 12f);
		}

		// Token: 0x040031F2 RID: 12786
		private const float PainFactorLowerGaussianWidthFactor = 1.5f;

		// Token: 0x040031F3 RID: 12787
		private const float PainFactorUpperGaussianWidthFactor = 2.53f;

		// Token: 0x040031F4 RID: 12788
		private const float MaxPainFactor = 12f;
	}
}
