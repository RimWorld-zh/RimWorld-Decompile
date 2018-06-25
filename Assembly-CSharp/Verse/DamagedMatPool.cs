using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEE RID: 3310
	internal static class DamagedMatPool
	{
		// Token: 0x0400315C RID: 12636
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x0400315D RID: 12637
		private static readonly Color DamagedMatStartingColor = Color.red;

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060048DF RID: 18655 RVA: 0x00263FAC File Offset: 0x002623AC
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x00263FCC File Offset: 0x002623CC
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
	}
}
