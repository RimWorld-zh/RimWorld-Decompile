using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C06 RID: 3078
	public static class DebugSolidColorMats
	{
		// Token: 0x06004356 RID: 17238 RVA: 0x00239730 File Offset: 0x00237B30
		public static Material MaterialOf(Color col)
		{
			Material material;
			Material result;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				result = material;
			}
			else
			{
				material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
				DebugSolidColorMats.colorMatDict.Add(col, material);
				result = material;
			}
			return result;
		}

		// Token: 0x04002E0D RID: 11789
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
