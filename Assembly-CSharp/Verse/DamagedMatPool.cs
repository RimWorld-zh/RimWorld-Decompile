using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CEB RID: 3307
	internal static class DamagedMatPool
	{
		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060048DC RID: 18652 RVA: 0x00263BF0 File Offset: 0x00261FF0
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00263C10 File Offset: 0x00262010
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

		// Token: 0x04003155 RID: 12629
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x04003156 RID: 12630
		private static readonly Color DamagedMatStartingColor = Color.red;
	}
}
