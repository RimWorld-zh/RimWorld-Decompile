using System;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class DebugMatsRandom
	{
		private static readonly Material[] mats = new Material[100];

		public const int MaterialCount = 100;

		private const float Opacity = 0.25f;

		static DebugMatsRandom()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsRandom.mats[i] = SolidColorMaterials.SimpleSolidColorMaterial(new Color(Rand.Value, Rand.Value, Rand.Value, 0.25f), false);
			}
		}

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
