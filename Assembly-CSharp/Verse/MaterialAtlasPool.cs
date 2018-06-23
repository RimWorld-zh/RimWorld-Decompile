using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D6D RID: 3437
	public static class MaterialAtlasPool
	{
		// Token: 0x04003372 RID: 13170
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x06004D1D RID: 19741 RVA: 0x00283158 File Offset: 0x00281558
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x02000D6E RID: 3438
		private class MaterialAtlas
		{
			// Token: 0x04003373 RID: 13171
			protected Material[] subMats = new Material[16];

			// Token: 0x04003374 RID: 13172
			private const float TexPadding = 0.03125f;

			// Token: 0x06004D1F RID: 19743 RVA: 0x002831AC File Offset: 0x002815AC
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

			// Token: 0x06004D20 RID: 19744 RVA: 0x00283260 File Offset: 0x00281660
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
