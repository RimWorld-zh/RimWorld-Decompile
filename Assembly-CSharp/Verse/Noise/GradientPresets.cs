using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F80 RID: 3968
	public static class GradientPresets
	{
		// Token: 0x06005FAD RID: 24493 RVA: 0x00309DF4 File Offset: 0x003081F4
		static GradientPresets()
		{
			List<GradientColorKey> list = new List<GradientColorKey>();
			list.Add(new GradientColorKey(Color.black, 0f));
			list.Add(new GradientColorKey(Color.white, 1f));
			List<GradientColorKey> list2 = new List<GradientColorKey>();
			list2.Add(new GradientColorKey(Color.red, 0f));
			list2.Add(new GradientColorKey(Color.green, 0.5f));
			list2.Add(new GradientColorKey(Color.blue, 1f));
			List<GradientColorKey> list3 = new List<GradientColorKey>();
			list3.Add(new GradientColorKey(Color.red, 0f));
			list3.Add(new GradientColorKey(Color.green, 0.333333343f));
			list3.Add(new GradientColorKey(Color.blue, 0.6666667f));
			list3.Add(new GradientColorKey(Color.black, 1f));
			List<GradientAlphaKey> list4 = new List<GradientAlphaKey>();
			list4.Add(new GradientAlphaKey(0f, 0.6666667f));
			list4.Add(new GradientAlphaKey(1f, 1f));
			List<GradientColorKey> list5 = new List<GradientColorKey>();
			list5.Add(new GradientColorKey(new Color(0f, 0f, 0.5f), 0f));
			list5.Add(new GradientColorKey(new Color(0.125f, 0.25f, 0.5f), 0.4f));
			list5.Add(new GradientColorKey(new Color(0.25f, 0.375f, 0.75f), 0.48f));
			list5.Add(new GradientColorKey(new Color(0f, 0.75f, 0f), 0.5f));
			list5.Add(new GradientColorKey(new Color(0.75f, 0.75f, 0f), 0.625f));
			list5.Add(new GradientColorKey(new Color(0.625f, 0.375f, 0.25f), 0.75f));
			list5.Add(new GradientColorKey(new Color(0.5f, 1f, 1f), 0.875f));
			list5.Add(new GradientColorKey(Color.white, 1f));
			List<GradientAlphaKey> list6 = new List<GradientAlphaKey>();
			list6.Add(new GradientAlphaKey(1f, 0f));
			list6.Add(new GradientAlphaKey(1f, 1f));
			GradientPresets._empty = new Gradient();
			GradientPresets._rgb = new Gradient();
			GradientPresets._rgb.SetKeys(list2.ToArray(), list6.ToArray());
			GradientPresets._rgba = new Gradient();
			GradientPresets._rgba.SetKeys(list3.ToArray(), list4.ToArray());
			GradientPresets._grayscale = new Gradient();
			GradientPresets._grayscale.SetKeys(list.ToArray(), list6.ToArray());
			GradientPresets._terrain = new Gradient();
			GradientPresets._terrain.SetKeys(list5.ToArray(), list6.ToArray());
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06005FAE RID: 24494 RVA: 0x0030A0E4 File Offset: 0x003084E4
		public static Gradient Empty
		{
			get
			{
				return GradientPresets._empty;
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06005FAF RID: 24495 RVA: 0x0030A100 File Offset: 0x00308500
		public static Gradient Grayscale
		{
			get
			{
				return GradientPresets._grayscale;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005FB0 RID: 24496 RVA: 0x0030A11C File Offset: 0x0030851C
		public static Gradient RGB
		{
			get
			{
				return GradientPresets._rgb;
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x0030A138 File Offset: 0x00308538
		public static Gradient RGBA
		{
			get
			{
				return GradientPresets._rgba;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x0030A154 File Offset: 0x00308554
		public static Gradient Terrain
		{
			get
			{
				return GradientPresets._terrain;
			}
		}

		// Token: 0x04003EDD RID: 16093
		private static Gradient _empty;

		// Token: 0x04003EDE RID: 16094
		private static Gradient _grayscale;

		// Token: 0x04003EDF RID: 16095
		private static Gradient _rgb;

		// Token: 0x04003EE0 RID: 16096
		private static Gradient _rgba;

		// Token: 0x04003EE1 RID: 16097
		private static Gradient _terrain;
	}
}
