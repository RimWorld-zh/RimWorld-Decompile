using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C08 RID: 3080
	public static class DebugSolidColorMats
	{
		// Token: 0x04002E0D RID: 11789
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();

		// Token: 0x06004359 RID: 17241 RVA: 0x0023980C File Offset: 0x00237C0C
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
	}
}
