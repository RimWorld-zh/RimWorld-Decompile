using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	internal static class DamagedMatPool
	{
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		private static readonly Color DamagedMatStartingColor = Color.red;

		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static DamagedMatPool()
		{
		}
	}
}
