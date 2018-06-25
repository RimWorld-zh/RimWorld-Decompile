using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0A RID: 3082
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x04002E12 RID: 11794
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04002E13 RID: 11795
		public const int MaterialCount = 100;

		// Token: 0x04002E14 RID: 11796
		private const float Opacity = 0.25f;

		// Token: 0x0600435E RID: 17246 RVA: 0x00239A48 File Offset: 0x00237E48
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00239AA0 File Offset: 0x00237EA0
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
