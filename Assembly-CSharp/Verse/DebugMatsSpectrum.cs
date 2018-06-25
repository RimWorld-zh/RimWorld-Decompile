using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0A RID: 3082
	[StaticConstructorOnStartup]
	public static class DebugMatsSpectrum
	{
		// Token: 0x04002E15 RID: 11797
		private static readonly Material[] spectrumMatsTranparent = new Material[100];

		// Token: 0x04002E16 RID: 11798
		private static readonly Material[] spectrumMatsOpaque = new Material[100];

		// Token: 0x04002E17 RID: 11799
		public const int MaterialCount = 100;

		// Token: 0x04002E18 RID: 11800
		public static Color[] DebugSpectrum = new Color[]
		{
			new Color(0.75f, 0f, 0f),
			new Color(0.5f, 0.3f, 0f),
			new Color(0f, 1f, 0f),
			new Color(0f, 0f, 1f),
			new Color(0.7f, 0f, 1f)
		};

		// Token: 0x0600435B RID: 17243 RVA: 0x00239B40 File Offset: 0x00237F40
		static DebugMatsSpectrum()
		{
			for (int i = 0; i < 100; i++)
			{
				DebugMatsSpectrum.spectrumMatsTranparent[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(0.25f), (float)i / 100f);
				DebugMatsSpectrum.spectrumMatsOpaque[i] = MatsFromSpectrum.Get(DebugMatsSpectrum.DebugSpectrumWithOpacity(1f), (float)i / 100f);
			}
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x00239C64 File Offset: 0x00238064
		private static Color[] DebugSpectrumWithOpacity(float opacity)
		{
			Color[] array = new Color[DebugMatsSpectrum.DebugSpectrum.Length];
			for (int i = 0; i < DebugMatsSpectrum.DebugSpectrum.Length; i++)
			{
				array[i] = new Color(DebugMatsSpectrum.DebugSpectrum[i].r, DebugMatsSpectrum.DebugSpectrum[i].g, DebugMatsSpectrum.DebugSpectrum[i].b, opacity);
			}
			return array;
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x00239CE4 File Offset: 0x002380E4
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
