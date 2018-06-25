using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000D6A RID: 3434
	public static class FadedMaterialPool
	{
		// Token: 0x04003361 RID: 13153
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04003362 RID: 13154
		private const int NumFadeSteps = 30;

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x002829D4 File Offset: 0x00280DD4
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004D02 RID: 19714 RVA: 0x002829F4 File Offset: 0x00280DF4
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

		// Token: 0x06004D03 RID: 19715 RVA: 0x00282A68 File Offset: 0x00280E68
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

		// Token: 0x06004D04 RID: 19716 RVA: 0x00282AFC File Offset: 0x00280EFC
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x02000D6B RID: 3435
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x04003363 RID: 13155
			private Material mat;

			// Token: 0x04003364 RID: 13156
			private int alphaIndex;

			// Token: 0x06004D06 RID: 19718 RVA: 0x00282B3B File Offset: 0x00280F3B
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06004D07 RID: 19719 RVA: 0x00282B4C File Offset: 0x00280F4C
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06004D08 RID: 19720 RVA: 0x00282B88 File Offset: 0x00280F88
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06004D09 RID: 19721 RVA: 0x00282BC8 File Offset: 0x00280FC8
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06004D0A RID: 19722 RVA: 0x00282BF4 File Offset: 0x00280FF4
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004D0B RID: 19723 RVA: 0x00282C14 File Offset: 0x00281014
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}
		}

		// Token: 0x02000D6C RID: 3436
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x04003365 RID: 13157
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();

			// Token: 0x06004D0D RID: 19725 RVA: 0x00282C3C File Offset: 0x0028103C
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06004D0E RID: 19726 RVA: 0x00282C5C File Offset: 0x0028105C
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
