using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0A RID: 3082
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x0600434F RID: 17231 RVA: 0x002383BC File Offset: 0x002367BC
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x002384E0 File Offset: 0x002368E0
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x00238560 File Offset: 0x00236960
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

		// Token: 0x04002E04 RID: 11780
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04002E05 RID: 11781
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04002E06 RID: 11782
		public const int MaterialCount = 100;

		// Token: 0x04002E07 RID: 11783
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
