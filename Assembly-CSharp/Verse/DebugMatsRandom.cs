using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0B RID: 3083
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06004352 RID: 17234 RVA: 0x002385A4 File Offset: 0x002369A4
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x002385FC File Offset: 0x002369FC
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04002E08 RID: 11784
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04002E09 RID: 11785
		public const int MaterialCount = 100;

		// Token: 0x04002E0A RID: 11786
		private const float Opacity = 0.25f;
	}
}
