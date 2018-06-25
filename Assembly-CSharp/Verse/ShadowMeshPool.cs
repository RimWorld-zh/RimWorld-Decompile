using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D76 RID: 3446
	public static class ShadowMeshPool
	{
		// Token: 0x04003380 RID: 13184
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();

		// Token: 0x06004D45 RID: 19781 RVA: 0x00284264 File Offset: 0x00282664
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x00284290 File Offset: 0x00282690
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x002842B0 File Offset: 0x002826B0
		public static Mesh GetShadowMesh(float baseWidth, float baseHeight, float tallness)
		{
			int key = ShadowMeshPool.HashOf(baseWidth, baseHeight, tallness);
			Mesh mesh;
			if (!ShadowMeshPool.shadowMeshDict.TryGetValue(key, out mesh))
			{
				mesh = MeshMakerShadows.NewShadowMesh(baseWidth, baseHeight, tallness);
				ShadowMeshPool.shadowMeshDict.Add(key, mesh);
			}
			return mesh;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x002842F8 File Offset: 0x002826F8
		private static int HashOf(float baseWidth, float baseheight, float tallness)
		{
			int num = (int)(baseWidth * 1000f);
			int num2 = (int)(baseheight * 1000f);
			int num3 = (int)(tallness * 1000f);
			int num4 = num * 391 ^ 261231;
			num4 ^= num2 * 612331;
			return num4 ^ num3 * 456123;
		}
	}
}
