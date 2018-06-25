using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C09 RID: 3081
	public static class DebugSolidColorMats
	{
		// Token: 0x04002E14 RID: 11796
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();

		// Token: 0x06004359 RID: 17241 RVA: 0x00239AEC File Offset: 0x00237EEC
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
