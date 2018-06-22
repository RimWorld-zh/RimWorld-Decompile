using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000D68 RID: 3432
	public static class FadedMaterialPool
	{
		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004CFD RID: 19709 RVA: 0x002828A8 File Offset: 0x00280CA8
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06004CFE RID: 19710 RVA: 0x002828C8 File Offset: 0x00280CC8
		public static long TotalMaterialBytes
		{
			get
			{
				long num = 0L;
				foreach (KeyValuePair<FadedMaterialPool.FadedMatRequest, Material> keyValuePair in FadedMaterialPool.cachedMats)
				{
					num += Profiler.GetRuntimeMemorySizeLong(keyValuePair.Value);
				}
				return num;
			}
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0028293C File Offset: 0x00280D3C
		public static Material FadedVersionOf(Material sourceMat, float alpha)
		{
			int num = FadedMaterialPool.IndexFromAlpha(alpha);
			Material result;
			if (num == 0)
			{
				result = BaseContent.ClearMat;
			}
			else if (num == 29)
			{
				result = sourceMat;
			}
			else
			{
				FadedMaterialPool.FadedMatRequest key = new FadedMaterialPool.FadedMatRequest(sourceMat, num);
				Material material;
				if (!FadedMaterialPool.cachedMats.TryGetValue(key, out material))
				{
					material = MaterialAllocator.Create(sourceMat);
					material.color = new Color(1f, 1f, 1f, (float)FadedMaterialPool.IndexFromAlpha(alpha) / 30f);
					FadedMaterialPool.cachedMats.Add(key, material);
				}
				result = material;
			}
			return result;
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x002829D0 File Offset: 0x00280DD0
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x04003361 RID: 13153
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04003362 RID: 13154
		private const int NumFadeSteps = 30;

		// Token: 0x02000D69 RID: 3433
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06004D02 RID: 19714 RVA: 0x00282A0F File Offset: 0x00280E0F
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06004D03 RID: 19715 RVA: 0x00282A20 File Offset: 0x00280E20
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06004D04 RID: 19716 RVA: 0x00282A5C File Offset: 0x00280E5C
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06004D05 RID: 19717 RVA: 0x00282A9C File Offset: 0x00280E9C
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06004D06 RID: 19718 RVA: 0x00282AC8 File Offset: 0x00280EC8
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004D07 RID: 19719 RVA: 0x00282AE8 File Offset: 0x00280EE8
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x04003363 RID: 13155
			private Material mat;

			// Token: 0x04003364 RID: 13156
			private int alphaIndex;
		}

		// Token: 0x02000D6A RID: 3434
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06004D09 RID: 19721 RVA: 0x00282B10 File Offset: 0x00280F10
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06004D0A RID: 19722 RVA: 0x00282B30 File Offset: 0x00280F30
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04003365 RID: 13157
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
