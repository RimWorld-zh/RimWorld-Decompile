using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0B RID: 3083
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x06004351 RID: 17233 RVA: 0x002383E4 File Offset: 0x002367E4
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x00238508 File Offset: 0x00236908
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x00238588 File Offset: 0x00236988
		public static Material Mat(int ind, bool transparent)
		{
			if (ind >= 100)
			{
				ind = 99;
			}
			if (ind < 0)
			{
				ind = 0;
			}
			return (!transparent) ? DebugMatsSpectrum.spectrumMatsOpaque[ind] : DebugMatsSpectrum.spectrumMatsTranparent[ind];
		}

		// Token: 0x04002E06 RID: 11782
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04002E07 RID: 11783
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04002E08 RID: 11784
		public const int MaterialCount = 100;

		// Token: 0x04002E09 RID: 11785
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};
	}
}
