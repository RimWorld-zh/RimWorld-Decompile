using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C0A RID: 3082
	public static class DebugSolidColorMats
	{
		// Token: 0x0600434F RID: 17231 RVA: 0x00238390 File Offset: 0x00236790
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

		// Token: 0x04002E05 RID: 11781
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
