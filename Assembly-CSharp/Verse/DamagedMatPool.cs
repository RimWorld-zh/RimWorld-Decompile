using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEE RID: 3310
	internal static class DamagedMatPool
	{
		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x002627D8 File Offset: 0x00260BD8
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x002627F8 File Offset: 0x00260BF8
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

		// Token: 0x0400314A RID: 12618
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x0400314B RID: 12619
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}
