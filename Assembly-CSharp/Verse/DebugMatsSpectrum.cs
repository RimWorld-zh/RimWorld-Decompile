using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C09 RID: 3081
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x04002E0E RID: 11790
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04002E0F RID: 11791
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04002E10 RID: 11792
		public const int MaterialCount = 100;

		// Token: 0x04002E11 RID: 11793
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};

		// Token: 0x0600435B RID: 17243 RVA: 0x00239860 File Offset: 0x00237C60
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x00239984 File Offset: 0x00237D84
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x00239A04 File Offset: 0x00237E04
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
	}
}
