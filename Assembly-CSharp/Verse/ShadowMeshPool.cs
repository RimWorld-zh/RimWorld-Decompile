using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D78 RID: 3448
	public static class ShadowMeshPool
	{
		// Token: 0x06004D2E RID: 19758 RVA: 0x00282BA8 File Offset: 0x00280FA8
		public static Mesh GetShadowMesh(ShadowData sha)
		{
			return ShadowMeshPool.GetShadowMesh(sha.BaseX, sha.BaseZ, sha.BaseY);
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x00282BD4 File Offset: 0x00280FD4
		public static Mesh GetShadowMesh(float baseEdgeLength, float tallness)
		{
			return ShadowMeshPool.GetShadowMesh(baseEdgeLength, baseEdgeLength, tallness);
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x00282BF4 File Offset: 0x00280FF4
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

		// Token: 0x06004D31 RID: 19761 RVA: 0x00282C3C File Offset: 0x0028103C
		private static int HashOf(float baseWidth, float baseheight, float tallness)
		{
			int num = (int)(baseWidth * 1000f);
			int num2 = (int)(baseheight * 1000f);
			int num3 = (int)(tallness * 1000f);
			int num4 = num * 391 ^ 261231;
			num4 ^= num2 * 612331;
			return num4 ^ num3 * 456123;
		}

		// Token: 0x04003377 RID: 13175
		private static Dictionary<int, Mesh> shadowMeshDict = new Dictionary<int, Mesh>();
	}
}
