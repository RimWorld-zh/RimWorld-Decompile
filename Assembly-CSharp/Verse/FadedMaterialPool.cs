using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public static class FadedMaterialPool
	{
		private struct FadedMatRequest : IEquatable<FadedMatRequest>
		{
			private Material mat;

			private int alphaIndex;

			public FadedMatRequest(Material mat, int alphaIndex)
			{
				this.mat = mat;
				this.alphaIndex = alphaIndex;
			}

			public override bool Equals(object obj)
			{
				return obj != null && obj is FadedMatRequest && this.Equals((FadedMatRequest)obj);
			}

			public bool Equals(FadedMatRequest other)
			{
				return (UnityEngine.Object)this.mat == (UnityEngine.Object)other.mat && this.alphaIndex == other.alphaIndex;
			}

			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.mat.GetHashCode(), this.alphaIndex);
			}

			public static bool operator ==(FadedMatRequest lhs, FadedMatRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(FadedMatRequest lhs, FadedMatRequest rhs)
			{
				return !(lhs == rhs);
			}
		}

		private class FadedMatRequestComparer : IEqualityComparer<FadedMatRequest>
		{
			public static readonly FadedMatRequestComparer Instance = new FadedMatRequestComparer();

			public bool Equals(FadedMatRequest x, FadedMatRequest y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(FadedMatRequest obj)
			{
				return obj.GetHashCode();
			}
		}

		private static Dictionary<FadedMatRequest, Material> cachedMats = new Dictionary<FadedMatRequest, Material>(FadedMatRequestComparer.Instance);

		private const int NumFadeSteps = 30;

		public static int TotalMaterialCount
		{
			get
			{
				return FadedMaterialPool.cachedMats.Count;
			}
		}

		public static long TotalMaterialBytes
		{
			get
			{
				long num = 0L;
				foreach (KeyValuePair<FadedMatRequest, Material> cachedMat in FadedMaterialPool.cachedMats)
				{
					num += Profiler.GetRuntimeMemorySizeLong(cachedMat.Value);
				}
				return num;
			}
		}

		public static Material FadedVersionOf(Material sourceMat, float alpha)
		{
			int num = FadedMaterialPool.IndexFromAlpha(alpha);
			Material result;
			switch (num)
			{
			case 0:
			{
				result = BaseContent.ClearMat;
				break;
			}
			case 29:
			{
				result = sourceMat;
				break;
			}
			default:
			{
				FadedMatRequest key = new FadedMatRequest(sourceMat, num);
				Material material = default(Material);
				if (!FadedMaterialPool.cachedMats.TryGetValue(key, out material))
				{
					material = new Material(sourceMat);
					material.color = new Color(1f, 1f, 1f, (float)((float)FadedMaterialPool.IndexFromAlpha(alpha) / 30.0));
					FadedMaterialPool.cachedMats.Add(key, material);
				}
				result = material;
				break;
			}
			}
			return result;
		}

		private static int IndexFromAlpha(float alpha)
		{
			int num = Mathf.FloorToInt((float)(alpha * 30.0));
			if (num == 30)
			{
				num = 29;
			}
			return num;
		}
	}
}
