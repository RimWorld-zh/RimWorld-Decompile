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
		private static Func<Texture2D, Material> <>f__am$cache0;

		public static List<Material> MatsFromTexturesInFolder(string dirPath)
		{
			string path = "Textures/" + dirPath;
			return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
			select MaterialPool.MatFrom(tex)).ToList<Material>();
		}

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

		[CompilerGenerated]
		private static Material <MatsFromTexturesInFolder>m__0(Texture2D tex)
		{
			return MaterialPool.MatFrom(tex);
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
