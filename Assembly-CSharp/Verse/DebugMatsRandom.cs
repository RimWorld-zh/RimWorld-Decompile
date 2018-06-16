using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0C RID: 3084
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		// Token: 0x06004354 RID: 17236 RVA: 0x002385CC File Offset: 0x002369CC
		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00238624 File Offset: 0x00236A24
		public static Material Mat(int ind)
		{
			ind %= 100;
			if (ind < 0)
			{
				ind *= -1;
			}
			return DebugMatsRandom.mats[ind];
		}

		// Token: 0x04002E0A RID: 11786
		private static readonly Material[] mats = new Material[100];

		// Token: 0x04002E0B RID: 11787
		public const int MaterialCount = 100;

		// Token: 0x04002E0C RID: 11788
		private const float Opacity = 0.25f;
	}
}
