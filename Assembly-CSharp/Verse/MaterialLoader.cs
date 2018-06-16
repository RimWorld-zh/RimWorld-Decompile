using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D73 RID: 3443
	public static class MaterialLoader
	{
		// Token: 0x06004D0E RID: 19726 RVA: 0x00281D24 File Offset: 0x00280124
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x00281D84 File Offset: 0x00280184
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
