using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000D6C RID: 3436
	public static class FadedMaterialPool
	{
		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004CEA RID: 19690 RVA: 0x00281318 File Offset: 0x0027F718
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004CEB RID: 19691 RVA: 0x00281338 File Offset: 0x0027F738
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

		// Token: 0x06004CEC RID: 19692 RVA: 0x002813AC File Offset: 0x0027F7AC
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

		// Token: 0x06004CED RID: 19693 RVA: 0x00281440 File Offset: 0x0027F840
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x04003358 RID: 13144
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04003359 RID: 13145
		private const int NumFadeSteps = 30;

		// Token: 0x02000D6D RID: 3437
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06004CEF RID: 19695 RVA: 0x0028147F File Offset: 0x0027F87F
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06004CF0 RID: 19696 RVA: 0x00281490 File Offset: 0x0027F890
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06004CF1 RID: 19697 RVA: 0x002814CC File Offset: 0x0027F8CC
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06004CF2 RID: 19698 RVA: 0x0028150C File Offset: 0x0027F90C
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06004CF3 RID: 19699 RVA: 0x00281538 File Offset: 0x0027F938
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004CF4 RID: 19700 RVA: 0x00281558 File Offset: 0x0027F958
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400335A RID: 13146
			private Material mat;

			// Token: 0x0400335B RID: 13147
			private int alphaIndex;
		}

		// Token: 0x02000D6E RID: 3438
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x06004CF6 RID: 19702 RVA: 0x00281580 File Offset: 0x0027F980
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06004CF7 RID: 19703 RVA: 0x002815A0 File Offset: 0x0027F9A0
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x0400335C RID: 13148
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();
		}
	}
}
