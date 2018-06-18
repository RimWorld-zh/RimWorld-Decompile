using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058B RID: 1419
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x06001B12 RID: 6930 RVA: 0x000E85AC File Offset: 0x000E69AC
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = WorldMaterials.DebugTileRenderQueue;
			}
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000E8614 File Offset: 0x000E6A14
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}

		// Token: 0x04000FF3 RID: 4083
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x04000FF4 RID: 4084
		public const int MaterialCount = 100;

		// Token: 0x04000FF5 RID: 4085
		private const float Opacity = 0.25f;

		// Token: 0x04000FF6 RID: 4086
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;
	}
}
