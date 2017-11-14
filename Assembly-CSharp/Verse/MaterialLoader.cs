using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public static class MaterialLoader
	{
		[CompilerGenerated]
		private static Func<Texture2D, Material> _003C_003Ef__mg_0024cache0;

		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			return Resources.LoadAll(path, typeof(Texture2D)).Cast<Texture2D>().Select(MaterialPool.MatFrom)
				.ToList();
		}

		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault();
			if ((UnityEngine.Object)material == (UnityEngine.Object)null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending);
				return BaseContent.BadMat;
			}
			return material;
		}
	}
}
