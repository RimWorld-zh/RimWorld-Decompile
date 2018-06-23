using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D74 RID: 3444
	public static class ShadowMeshPool
	{
		// Token: 0x04003380 RID: 13184
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();

		// Token: 0x06004D41 RID: 19777 RVA: 0x00284138 File Offset: 0x00282538
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06004D42 RID: 19778 RVA: 0x00284164 File Offset: 0x00282564
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x00284184 File Offset: 0x00282584
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

		// Token: 0x06004D44 RID: 19780 RVA: 0x002841CC File Offset: 0x002825CC
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
