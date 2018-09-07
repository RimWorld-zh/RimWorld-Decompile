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
		private static Func<Texture2D, Material> <>f__mg$cache0;

		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			IEnumerable<Texture2D> source = Resources.LoadAll(path, typeof(Texture2D)).Cast<Texture2D>();
			if (MaterialLoader.<>f__mg$cache0 == null)
			{
				MaterialLoader.<>f__mg$cache0 = new Func<Texture2D, Material>(MaterialPool.MatFrom);
			}
			return source.Select(MaterialLoader.<>f__mg$cache0).ToList<Material>();
		}

		public static Material MatWithEnding(string dirPath, string ending)
		{
			Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
			where mat.mainTexture.name.ToLower().EndsWith(ending)
			select mat).FirstOrDefault<Material>();
			if (material == null)
			{
				Log.Warning("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending, false);
				return BaseContent.BadMat;
			}
			return material;
		}

		[CompilerGenerated]
		private sealed class <MatWithEnding>c__AnonStorey0
		{
			internal string ending;

			public <MatWithEnding>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Material mat)
			{
				return mat.mainTexture.name.ToLower().EndsWith(this.ending);
			}
		}
	}
}
