using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D71 RID: 3441
	public static class MaterialLoader
	{
		// Token: 0x06004D25 RID: 19749 RVA: 0x002833E0 File Offset: 0x002817E0
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x00283440 File Offset: 0x00281840
		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault<Material>();
			Material result;
			if (material == null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending, false);
				result = BaseContent.BadMat;
			}
			else
			{
				result = material;
			}
			return result;
		}
	}
}
