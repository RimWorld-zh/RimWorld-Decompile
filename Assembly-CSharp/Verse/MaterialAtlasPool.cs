using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D71 RID: 3441
	public static class MaterialAtlasPool
	{
		// Token: 0x06004D0A RID: 19722 RVA: 0x00281BC8 File Offset: 0x0027FFC8
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04003369 RID: 13161
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x02000D72 RID: 3442
		private class MaterialAtlas
		{
			// Token: 0x06004D0C RID: 19724 RVA: 0x00281C1C File Offset: 0x0028001C
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

			// Token: 0x06004D0D RID: 19725 RVA: 0x00281CD0 File Offset: 0x002800D0
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

			// Token: 0x0400336A RID: 13162
			protected Material[] subMats = new Material[16];

			// Token: 0x0400336B RID: 13163
			private const float TexPadding = 0.03125f;
		}
	}
}
