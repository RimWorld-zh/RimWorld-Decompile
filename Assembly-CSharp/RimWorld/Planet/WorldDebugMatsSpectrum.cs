using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000587 RID: 1415
	[StaticConstructorOnStartup]
	public static class WorldDebugMatsSpectrum
	{
		// Token: 0x04000FF0 RID: 4080
		private static readonly Material[] spectrumMats = new Material[100];

		// Token: 0x04000FF1 RID: 4081
		public const int MaterialCount = 100;

		// Token: 0x04000FF2 RID: 4082
		private const float Opacity = 0.25f;

		// Token: 0x04000FF3 RID: 4083
		private static readonly Color[] DebugSpectrum = DebugMatsSpectrum.DebugSpectrum;

		// Token: 0x06001B09 RID: 6921 RVA: 0x000E8600 File Offset: 0x000E6A00
		static WorldDebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				WorldDebugMatsSpectrum.spectrumMats[i] = MatsFromSpectrum.Get(WorldDebugMatsSpectrum.DebugSpectrum, (float)i / 100f, ShaderDatabase.WorldOverlayTransparent);
				WorldDebugMatsSpectrum.spectrumMats[i].renderQueue = WorldMaterials.DebugTileRenderQueue;
			}
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000E8668 File Offset: 0x000E6A68
		public static Material Mat(int ind)
		{
			ind = Mathf.Clamp(ind, 0, 99);
			return WorldDebugMatsSpectrum.spectrumMats[ind];
		}
	}
}
