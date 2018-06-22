using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D6F RID: 3439
	public static class MaterialLoader
	{
		// Token: 0x06004D21 RID: 19745 RVA: 0x002832B4 File Offset: 0x002816B4
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x00283314 File Offset: 0x00281714
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
