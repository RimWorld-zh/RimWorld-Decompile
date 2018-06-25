using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D6F RID: 3439
	public static class MaterialAtlasPool
	{
		// Token: 0x04003372 RID: 13170
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x06004D21 RID: 19745 RVA: 0x00283284 File Offset: 0x00281684
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x02000D70 RID: 3440
		private class MaterialAtlas
		{
			// Token: 0x04003373 RID: 13171
			protected Material[] subMats = new Material[16];

			// Token: 0x04003374 RID: 13172
			private const float TexPadding = 0.03125f;

			// Token: 0x06004D23 RID: 19747 RVA: 0x002832D8 File Offset: 0x002816D8
			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)(i % 4) * 0.25f + 0.03125f;
					float y = (float)(i / 4) * 0.25f + 0.03125f;
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = MaterialAllocator.Create(newRootMat);
					material.name = newRootMat.name + "_ASM" + i;
					material.mainTextureScale = mainTextureScale;
					material.mainTextureOffset = mainTextureOffset;
					this.subMats[i] = material;
				}
			}

			// Token: 0x06004D24 RID: 19748 RVA: 0x0028338C File Offset: 0x0028178C
			public Material SubMat(LinkDirections linkSet)
			{
				Material result;
				if ((int)linkSet >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + (int)linkSet + ": out of range.", false);
					result = BaseContent.BadMat;
				}
				else
				{
					result = this.subMats[(int)linkSet];
				}
				return result;
			}
		}
	}
}
