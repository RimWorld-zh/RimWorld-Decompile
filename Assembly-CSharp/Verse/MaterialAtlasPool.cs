using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class MaterialAtlasPool
	{
		private class MaterialAtlas
		{
			protected Material[] subMats = new Material[16];

			private const float TexPadding = 0.03125f;

			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)((float)(i % 4) * 0.25 + 0.03125);
					float y = (float)((float)(i / 4) * 0.25 + 0.03125);
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = new Material(newRootMat)
					{
						name = newRootMat.name + "_ASM" + i,
						mainTextureScale = mainTextureScale,
						mainTextureOffset = mainTextureOffset
					};
					this.subMats[i] = material;
				}
			}

			public Material SubMat(LinkDirections linkSet)
			{
				Material result;
				if ((int)linkSet >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + (int)linkSet + ": out of range.");
					result = BaseContent.BadMat;
				}
				else
				{
					result = this.subMats[(uint)linkSet];
				}
				return result;
			}
		}

		private static Dictionary<Material, MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlas>();

		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}
	}
}
