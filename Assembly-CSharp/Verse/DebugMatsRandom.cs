using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0B RID: 3083
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x04002E19 RID: 11801
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04002E1A RID: 11802
		public const int MaterialCount = 100;

		// Token: 0x04002E1B RID: 11803
		private const float Opacity = 0.25f;

		// Token: 0x0600435E RID: 17246 RVA: 0x00239D28 File Offset: 0x00238128
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00239D80 File Offset: 0x00238180
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}
	}
}
