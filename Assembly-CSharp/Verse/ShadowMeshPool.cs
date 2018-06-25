using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D77 RID: 3447
	public static class ShadowMeshPool
	{
		// Token: 0x04003387 RID: 13191
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();

		// Token: 0x06004D45 RID: 19781 RVA: 0x00284544 File Offset: 0x00282944
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x00284570 File Offset: 0x00282970
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x00284590 File Offset: 0x00282990
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

		// Token: 0x06004D48 RID: 19784 RVA: 0x002845D8 File Offset: 0x002829D8
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
