using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C08 RID: 3080
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x0600435B RID: 17243 RVA: 0x0023996C File Offset: 0x00237D6C
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x002399C4 File Offset: 0x00237DC4
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04002E12 RID: 11794
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04002E13 RID: 11795
		public const int MaterialCount = 100;

		// Token: 0x04002E14 RID: 11796
		private const float Opacity = 0.25f;
	}
}
