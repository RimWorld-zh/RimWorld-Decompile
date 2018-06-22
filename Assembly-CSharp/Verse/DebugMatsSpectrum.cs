using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C07 RID: 3079
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x06004358 RID: 17240 RVA: 0x00239784 File Offset: 0x00237B84
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x002398A8 File Offset: 0x00237CA8
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x00239928 File Offset: 0x00237D28
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
	}
}
