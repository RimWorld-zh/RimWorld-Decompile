using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C09 RID: 3081
	public static class DebugSolidColorMats
	{
		// Token: 0x0600434D RID: 17229 RVA: 0x00238368 File Offset: 0x00236768
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

		// Token: 0x04002E03 RID: 11779
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
