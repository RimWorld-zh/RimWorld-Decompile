using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public static class MaterialLoader
	{
		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList();
		}

		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault();
			Material result;
			if ((UnityEngine.Object)material == (UnityEngine.Object)null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending);
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
