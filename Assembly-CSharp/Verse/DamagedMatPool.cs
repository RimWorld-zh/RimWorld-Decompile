using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEF RID: 3311
	internal static class DamagedMatPool
	{
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060048CD RID: 18637 RVA: 0x00262800 File Offset: 0x00260C00
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x00262820 File Offset: 0x00260C20
		public static Material GetDamageFlashMat(Material baseMat, float damPct)
		{
			Material result;
			if (damPct < 0.01f)
			{
				result = baseMat;
			}
			else
			{
				Material material;
				if (!DamagedMatPool.damagedMats.TryGetValue(baseMat, out material))
				{
					material = MaterialAllocator.Create(baseMat);
					DamagedMatPool.damagedMats.Add(baseMat, material);
				}
				Color color = Color.Lerp(baseMat.color, DamagedMatPool.DamagedMatStartingColor, damPct);
				material.color = color;
				result = material;
			}
			return result;
		}

		// Token: 0x0400314C RID: 12620
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x0400314D RID: 12621
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}
