using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000D6B RID: 3435
	public static class FadedMaterialPool
	{
		// Token: 0x04003368 RID: 13160
		private static Dictionary<FadedMaterialPool.FadedMatRequest, Material> cachedMats = new Dictionary<FadedMaterialPool.FadedMatRequest, Material>(FadedMaterialPool.FadedMatRequestComparer.Instance);

		// Token: 0x04003369 RID: 13161
		private const int NumFadeSteps = 30;

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x00282CB4 File Offset: 0x002810B4
		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06004D02 RID: 19714 RVA: 0x00282CD4 File Offset: 0x002810D4
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

		// Token: 0x06004D03 RID: 19715 RVA: 0x00282D48 File Offset: 0x00281148
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

		// Token: 0x06004D04 RID: 19716 RVA: 0x00282DDC File Offset: 0x002811DC
		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt(alpha * 30f);
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}

		// Token: 0x02000D6C RID: 3436
		private struct FadedMatRequest : IEquatable<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x0400336A RID: 13162
			private Material mat;

			// Token: 0x0400336B RID: 13163
			private int alphaIndex;

			// Token: 0x06004D06 RID: 19718 RVA: 0x00282E1B File Offset: 0x0028121B
			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			// Token: 0x06004D07 RID: 19719 RVA: 0x00282E2C File Offset: 0x0028122C
			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMaterialPool.FadedMatRequest && this.Equals((FadedMaterialPool.FadedMatRequest)obj);
			}

			// Token: 0x06004D08 RID: 19720 RVA: 0x00282E68 File Offset: 0x00281268
			public bool Equals(FadedMaterialPool.FadedMatRequest other)
			{
				return this.mat == other.mat && this.alphaIndex == other.alphaIndex;
			}

			// Token: 0x06004D09 RID: 19721 RVA: 0x00282EA8 File Offset: 0x002812A8
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			// Token: 0x06004D0A RID: 19722 RVA: 0x00282ED4 File Offset: 0x002812D4
			public static bool operator ==(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004D0B RID: 19723 RVA: 0x00282EF4 File Offset: 0x002812F4
			public static bool operator !=(FadedMaterialPool.FadedMatRequest lhs, FadedMaterialPool.FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}
		}

		// Token: 0x02000D6D RID: 3437
		private class FadedMatRequestComparer : IEqualityComparer<FadedMaterialPool.FadedMatRequest>
		{
			// Token: 0x0400336C RID: 13164
			public static readonly FadedMaterialPool.FadedMatRequestComparer Instance = new FadedMaterialPool.FadedMatRequestComparer();

			// Token: 0x06004D0D RID: 19725 RVA: 0x00282F1C File Offset: 0x0028131C
			public bool Equals(FadedMaterialPool.FadedMatRequest x, FadedMaterialPool.FadedMatRequest y)
			{
				return x.Equals(y);
			}

			// Token: 0x06004D0E RID: 19726 RVA: 0x00282F3C File Offset: 0x0028133C
			public int GetHashCode(FadedMaterialPool.FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
