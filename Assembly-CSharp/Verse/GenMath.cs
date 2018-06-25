using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F48 RID: 3912
	[HasDebugOutput]
	public static class GenMath
	{
		// Token: 0x04003E21 RID: 15905
		public const float BigEpsilon = 1E-07f;

		// Token: 0x04003E22 RID: 15906
		public const float Sqrt2 = 1.41421354f;

		// Token: 0x04003E23 RID: 15907
		private static List<float> tmpElements = new List<float>();

		// Token: 0x04003E24 RID: 15908
		private static List<Pair<float, float>> tmpPairs = new List<Pair<float, float>>();

		// Token: 0x04003E25 RID: 15909
		private static List<float> tmpScores = new List<float>();

		// Token: 0x04003E26 RID: 15910
		private static List<float> tmpCalcList = new List<float>();

		// Token: 0x06005E6D RID: 24173 RVA: 0x00300560 File Offset: 0x002FE960
		public static float RoundedHundredth(float f)
		{
			return Mathf.Round(f * 100f) / 100f;
		}

		// Token: 0x06005E6E RID: 24174 RVA: 0x00300588 File Offset: 0x002FE988
		public static int RoundTo(int value, int roundToNearest)
		{
			return (int)Math.Round((double)((float)value / (float)roundToNearest)) * roundToNearest;
		}

		// Token: 0x06005E6F RID: 24175 RVA: 0x003005AC File Offset: 0x002FE9AC
		public static float RoundTo(float value, float roundToNearest)
		{
			return (float)((int)Math.Round((double)(value / roundToNearest))) * roundToNearest;
		}

		// Token: 0x06005E70 RID: 24176 RVA: 0x003005D0 File Offset: 0x002FE9D0
		public static float ChanceEitherHappens(float chanceA, float chanceB)
		{
			return chanceA + (1f - chanceA) * chanceB;
		}

		// Token: 0x06005E71 RID: 24177 RVA: 0x003005F0 File Offset: 0x002FE9F0
		public static float SmootherStep(float edge0, float edge1, float x)
		{
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * x * (x * (x * 6f - 15f) + 10f);
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x00300630 File Offset: 0x002FEA30
		public static int RoundRandom(float f)
		{
			return (int)f + ((Rand.Value >= f % 1f) ? 0 : 1);
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x00300660 File Offset: 0x002FEA60
		public static float WeightedAverage(float A, float weightA, float B, float weightB)
		{
			return (A * weightA + B * weightB) / (weightA + weightB);
		}

		// Token: 0x06005E74 RID: 24180 RVA: 0x00300680 File Offset: 0x002FEA80
		public static float Median<T>(IList<T> list, Func<T, float> orderBy, float noneValue = 0f, float center = 0.5f)
		{
			float result;
			if (list.NullOrEmpty<T>())
			{
				result = noneValue;
			}
			else
			{
				GenMath.tmpElements.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					GenMath.tmpElements.Add(orderBy(list[i]));
				}
				GenMath.tmpElements.Sort();
				result = GenMath.tmpElements[Mathf.Min(Mathf.FloorToInt((float)GenMath.tmpElements.Count * center), GenMath.tmpElements.Count - 1)];
			}
			return result;
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x00300718 File Offset: 0x002FEB18
		public static float WeightedMedian(IList<Pair<float, float>> list, float noneValue = 0f, float center = 0.5f)
		{
			GenMath.tmpPairs.Clear();
			GenMath.tmpPairs.AddRange(list);
			float num = 0f;
			for (int i = 0; i < GenMath.tmpPairs.Count; i++)
			{
				float second = GenMath.tmpPairs[i].Second;
				if (second < 0f)
				{
					Log.ErrorOnce("Negative weight in WeightedMedian: " + second, GenMath.tmpPairs.GetHashCode(), false);
				}
				else
				{
					num += second;
				}
			}
			float result;
			if (num <= 0f)
			{
				result = noneValue;
			}
			else
			{
				GenMath.tmpPairs.SortBy((Pair<float, float> x) => x.First);
				float num2 = 0f;
				for (int j = 0; j < GenMath.tmpPairs.Count; j++)
				{
					float first = GenMath.tmpPairs[j].First;
					float second2 = GenMath.tmpPairs[j].Second;
					num2 += second2 / num;
					if (num2 >= center)
					{
						return first;
					}
				}
				result = GenMath.tmpPairs.Last<Pair<float, float>>().First;
			}
			return result;
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x00300870 File Offset: 0x002FEC70
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x00300890 File Offset: 0x002FEC90
		public static float LerpDouble(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			float num = (x - inFrom) / (inTo - inFrom);
			return outFrom + (outTo - outFrom) * num;
		}

		// Token: 0x06005E78 RID: 24184 RVA: 0x003008B8 File Offset: 0x002FECB8
		public static float LerpDoubleClamped(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			return GenMath.LerpDouble(inFrom, inTo, outFrom, outTo, Mathf.Clamp(x, Mathf.Min(inFrom, inTo), Mathf.Max(inFrom, inTo)));
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x003008EC File Offset: 0x002FECEC
		public static float Reflection(float value, float mirror)
		{
			return mirror - (value - mirror);
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x00300908 File Offset: 0x002FED08
		public static Quaternion ToQuat(this float ang)
		{
			return Quaternion.AngleAxis(ang, Vector3.up);
		}

		// Token: 0x06005E7B RID: 24187 RVA: 0x00300928 File Offset: 0x002FED28
		public static float GetFactorInInterval(float min, float mid, float max, float power, float x)
		{
			float result;
			if (min > max)
			{
				result = 0f;
			}
			else if (x <= min || x >= max)
			{
				result = 0f;
			}
			else if (x == mid)
			{
				result = 1f;
			}
			else
			{
				float f;
				if (x < mid)
				{
					f = 1f - (mid - x) / (mid - min);
				}
				else
				{
					f = 1f - (x - mid) / (max - mid);
				}
				result = Mathf.Pow(f, power);
			}
			return result;
		}

		// Token: 0x06005E7C RID: 24188 RVA: 0x003009B8 File Offset: 0x002FEDB8
		public static float FlatHill(float min, float lower, float upper, float max, float x)
		{
			float result;
			if (x < min)
			{
				result = 0f;
			}
			else if (x < lower)
			{
				result = Mathf.InverseLerp(min, lower, x);
			}
			else if (x < upper)
			{
				result = 1f;
			}
			else if (x < max)
			{
				result = Mathf.InverseLerp(max, upper, x);
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06005E7D RID: 24189 RVA: 0x00300A28 File Offset: 0x002FEE28
		public static float FlatHill(float minY, float min, float lower, float upper, float max, float maxY, float x)
		{
			float result;
			if (x < min)
			{
				result = minY;
			}
			else if (x < lower)
			{
				result = GenMath.LerpDouble(min, lower, minY, 1f, x);
			}
			else if (x < upper)
			{
				result = 1f;
			}
			else if (x < max)
			{
				result = GenMath.LerpDouble(upper, max, 1f, maxY, x);
			}
			else
			{
				result = maxY;
			}
			return result;
		}

		// Token: 0x06005E7E RID: 24190 RVA: 0x00300AA0 File Offset: 0x002FEEA0
		public static int OctileDistance(int dx, int dz, int cardinal, int diagonal)
		{
			return cardinal * (dx + dz) + (diagonal - 2 * cardinal) * Mathf.Min(dx, dz);
		}

		// Token: 0x06005E7F RID: 24191 RVA: 0x00300AC8 File Offset: 0x002FEEC8
		public static float UnboundedValueToFactor(float val)
		{
			float result;
			if (val > 0f)
			{
				result = 1f + val;
			}
			else
			{
				result = 1f / (1f - val);
			}
			return result;
		}

		// Token: 0x06005E80 RID: 24192 RVA: 0x00300B04 File Offset: 0x002FEF04
		[DebugOutput]
		[Category("System")]
		public static void TestMathPerf()
		{
			IntVec3 intVec = new IntVec3(72, 0, 65);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Math perf tests (" + 1E+07f + " tests each)");
			float num = 0f;
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num2 = 0;
			while ((float)num2 < 1E+07f)
			{
				num += (float)Math.Sqrt(101.20999908447266);
				num2++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"(float)System.Math.Sqrt(",
				101.21f,
				"): ",
				stopwatch.ElapsedTicks
			}));
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			int num3 = 0;
			while ((float)num3 < 1E+07f)
			{
				num += Mathf.Sqrt(101.21f);
				num3++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"UnityEngine.Mathf.Sqrt(",
				101.21f,
				"): ",
				stopwatch2.ElapsedTicks
			}));
			Stopwatch stopwatch3 = Stopwatch.StartNew();
			int num4 = 0;
			while ((float)num4 < 1E+07f)
			{
				num += GenMath.Sqrt(101.21f);
				num4++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Verse.GenMath.Sqrt(",
				101.21f,
				"): ",
				stopwatch3.ElapsedTicks
			}));
			Stopwatch stopwatch4 = Stopwatch.StartNew();
			int num5 = 0;
			while ((float)num5 < 1E+07f)
			{
				num += (float)intVec.LengthManhattan;
				num5++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthManhattan: " + stopwatch4.ElapsedTicks);
			Stopwatch stopwatch5 = Stopwatch.StartNew();
			int num6 = 0;
			while ((float)num6 < 1E+07f)
			{
				num += intVec.LengthHorizontal;
				num6++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontal: " + stopwatch5.ElapsedTicks);
			Stopwatch stopwatch6 = Stopwatch.StartNew();
			int num7 = 0;
			while ((float)num7 < 1E+07f)
			{
				num += (float)intVec.LengthHorizontalSquared;
				num7++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontalSquared: " + stopwatch6.ElapsedTicks);
			stringBuilder.AppendLine("total: " + num);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06005E81 RID: 24193 RVA: 0x00300DAC File Offset: 0x002FF1AC
		public static float Min(float a, float b, float c)
		{
			float result;
			if (a < b)
			{
				if (a < c)
				{
					result = a;
				}
				else
				{
					result = c;
				}
			}
			else if (b < c)
			{
				result = b;
			}
			else
			{
				result = c;
			}
			return result;
		}

		// Token: 0x06005E82 RID: 24194 RVA: 0x00300DF0 File Offset: 0x002FF1F0
		public static int Max(int a, int b, int c)
		{
			int result;
			if (a > b)
			{
				if (a > c)
				{
					result = a;
				}
				else
				{
					result = c;
				}
			}
			else if (b > c)
			{
				result = b;
			}
			else
			{
				result = c;
			}
			return result;
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x00300E34 File Offset: 0x002FF234
		public static float SphericalDistance(Vector3 normalizedA, Vector3 normalizedB)
		{
			float result;
			if (normalizedA == normalizedB)
			{
				result = 0f;
			}
			else
			{
				result = Mathf.Acos(Vector3.Dot(normalizedA, normalizedB));
			}
			return result;
		}

		// Token: 0x06005E84 RID: 24196 RVA: 0x00300E6C File Offset: 0x002FF26C
		public static void DHondtDistribution(List<int> candidates, Func<int, float> scoreGetter, int numToDistribute)
		{
			GenMath.tmpScores.Clear();
			GenMath.tmpCalcList.Clear();
			for (int i = 0; i < candidates.Count; i++)
			{
				float item = scoreGetter(i);
				candidates[i] = 0;
				GenMath.tmpScores.Add(item);
				GenMath.tmpCalcList.Add(item);
			}
			for (int j = 0; j < numToDistribute; j++)
			{
				int num = GenMath.tmpCalcList.IndexOf(GenMath.tmpCalcList.Max());
				int index;
				candidates[index = num] = candidates[index] + 1;
				GenMath.tmpCalcList[num] = GenMath.tmpScores[num] / ((float)candidates[num] + 1f);
			}
		}

		// Token: 0x06005E85 RID: 24197 RVA: 0x00300F34 File Offset: 0x002FF334
		public static int PositiveMod(int x, int m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06005E86 RID: 24198 RVA: 0x00300F50 File Offset: 0x002FF350
		public static long PositiveMod(long x, long m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06005E87 RID: 24199 RVA: 0x00300F6C File Offset: 0x002FF36C
		public static float PositiveMod(float x, float m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06005E88 RID: 24200 RVA: 0x00300F88 File Offset: 0x002FF388
		public static int PositiveModRemap(long x, int d, int m)
		{
			if (x < 0L)
			{
				x -= (long)(d - 1);
			}
			return (int)((x / (long)d % (long)m + (long)m) % (long)m);
		}

		// Token: 0x06005E89 RID: 24201 RVA: 0x00300FBC File Offset: 0x002FF3BC
		public static Vector3 BezierCubicEvaluate(float t, GenMath.BezierCubicControls bcc)
		{
			return GenMath.BezierCubicEvaluate(t, bcc.w0, bcc.w1, bcc.w2, bcc.w3);
		}

		// Token: 0x06005E8A RID: 24202 RVA: 0x00300FF4 File Offset: 0x002FF3F4
		public static Vector3 BezierCubicEvaluate(float t, Vector3 w0, Vector3 w1, Vector3 w2, Vector3 w3)
		{
			float d = t * t;
			float num = 1f - t;
			float d2 = num * num;
			return w0 * d2 * num + 3f * w1 * d2 * t + 3f * w2 * num * d + w3 * d * t;
		}

		// Token: 0x06005E8B RID: 24203 RVA: 0x00301074 File Offset: 0x002FF474
		public static float CirclesOverlapArea(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			float num = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
			float num2 = Mathf.Sqrt(num);
			float num3 = r1 * r1;
			float num4 = r2 * r2;
			float num5 = Mathf.Abs(r1 - r2);
			float result;
			if (num2 >= r1 + r2)
			{
				result = 0f;
			}
			else if (num2 <= num5 && r1 >= r2)
			{
				result = 3.14159274f * num4;
			}
			else if (num2 <= num5 && r2 >= r1)
			{
				result = 3.14159274f * num3;
			}
			else
			{
				float num6 = Mathf.Acos((num3 - num4 + num) / (2f * r1 * num2)) * 2f;
				float num7 = Mathf.Acos((num4 - num3 + num) / (2f * r2 * num2)) * 2f;
				float num8 = (num7 * num4 - num4 * Mathf.Sin(num7)) * 0.5f;
				float num9 = (num6 * num3 - num3 * Mathf.Sin(num6)) * 0.5f;
				result = num8 + num9;
			}
			return result;
		}

		// Token: 0x06005E8C RID: 24204 RVA: 0x0030117C File Offset: 0x002FF57C
		public static bool AnyIntegerInRange(float min, float max)
		{
			return Mathf.Ceil(min) <= max;
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x003011A0 File Offset: 0x002FF5A0
		public static void NormalizeToSum1(ref float a, ref float b, ref float c)
		{
			float num = a + b + c;
			if (num == 0f)
			{
				a = 1f;
				b = 0f;
				c = 0f;
			}
			else
			{
				a /= num;
				b /= num;
				c /= num;
			}
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x003011F4 File Offset: 0x002FF5F4
		public static float InverseLerp(float a, float b, float value)
		{
			float result;
			if (a == b)
			{
				result = ((value >= a) ? 1f : 0f);
			}
			else
			{
				result = Mathf.InverseLerp(a, b, value);
			}
			return result;
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x00301234 File Offset: 0x002FF634
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			T result;
			if (by1 >= by2 && by1 >= by3)
			{
				result = elem1;
			}
			else if (by2 >= by1 && by2 >= by3)
			{
				result = elem2;
			}
			else
			{
				result = elem3;
			}
			return result;
		}

		// Token: 0x06005E90 RID: 24208 RVA: 0x00301278 File Offset: 0x002FF678
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			T result;
			if (by1 >= by2 && by1 >= by3 && by1 >= by4)
			{
				result = elem1;
			}
			else if (by2 >= by1 && by2 >= by3 && by2 >= by4)
			{
				result = elem2;
			}
			else if (by3 >= by1 && by3 >= by2 && by3 >= by4)
			{
				result = elem3;
			}
			else
			{
				result = elem4;
			}
			return result;
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x003012EC File Offset: 0x002FF6EC
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			T result;
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5)
			{
				result = elem1;
			}
			else if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5)
			{
				result = elem2;
			}
			else if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5)
			{
				result = elem3;
			}
			else if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5)
			{
				result = elem4;
			}
			else
			{
				result = elem5;
			}
			return result;
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x003013A4 File Offset: 0x002FF7A4
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			T result;
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6)
			{
				result = elem1;
			}
			else if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6)
			{
				result = elem2;
			}
			else if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6)
			{
				result = elem3;
			}
			else if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6)
			{
				result = elem4;
			}
			else if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6)
			{
				result = elem5;
			}
			else
			{
				result = elem6;
			}
			return result;
		}

		// Token: 0x06005E93 RID: 24211 RVA: 0x003014B0 File Offset: 0x002FF8B0
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			T result;
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7)
			{
				result = elem1;
			}
			else if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7)
			{
				result = elem2;
			}
			else if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7)
			{
				result = elem3;
			}
			else if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7)
			{
				result = elem4;
			}
			else if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7)
			{
				result = elem5;
			}
			else if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7)
			{
				result = elem6;
			}
			else
			{
				result = elem7;
			}
			return result;
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00301624 File Offset: 0x002FFA24
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			T result;
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7 && by1 >= by8)
			{
				result = elem1;
			}
			else if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7 && by2 >= by8)
			{
				result = elem2;
			}
			else if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7 && by3 >= by8)
			{
				result = elem3;
			}
			else if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7 && by4 >= by8)
			{
				result = elem4;
			}
			else if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7 && by5 >= by8)
			{
				result = elem5;
			}
			else if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7 && by6 >= by8)
			{
				result = elem6;
			}
			else if (by7 >= by1 && by7 >= by2 && by7 >= by3 && by7 >= by4 && by7 >= by5 && by7 >= by6 && by7 >= by8)
			{
				result = elem7;
			}
			else
			{
				result = elem8;
			}
			return result;
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x00301810 File Offset: 0x002FFC10
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3);
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x00301838 File Offset: 0x002FFC38
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4);
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x00301864 File Offset: 0x002FFC64
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5);
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x00301894 File Offset: 0x002FFC94
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6);
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x003018C8 File Offset: 0x002FFCC8
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7);
		}

		// Token: 0x06005E9A RID: 24218 RVA: 0x00301904 File Offset: 0x002FFD04
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7, elem8, -by8);
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x00301944 File Offset: 0x002FFD44
		public static T MaxByRandomIfEqual<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8, float eps = 0.0001f)
		{
			return GenMath.MaxBy<T>(elem1, by1 + Rand.Range(0f, eps), elem2, by2 + Rand.Range(0f, eps), elem3, by3 + Rand.Range(0f, eps), elem4, by4 + Rand.Range(0f, eps), elem5, by5 + Rand.Range(0f, eps), elem6, by6 + Rand.Range(0f, eps), elem7, by7 + Rand.Range(0f, eps), elem8, by8 + Rand.Range(0f, eps));
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x003019E4 File Offset: 0x002FFDE4
		public static float Stddev(IEnumerable<float> data)
		{
			int num = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (float num4 in data)
			{
				float num5 = num4;
				num++;
				num2 += (double)num5;
				num3 += (double)(num5 * num5);
			}
			double num6 = num2 / (double)num;
			double num7 = num3 / (double)num - num6 * num6;
			return Mathf.Sqrt((float)num7);
		}

		// Token: 0x02000F49 RID: 3913
		public struct BezierCubicControls
		{
			// Token: 0x04003E28 RID: 15912
			public Vector3 w0;

			// Token: 0x04003E29 RID: 15913
			public Vector3 w1;

			// Token: 0x04003E2A RID: 15914
			public Vector3 w2;

			// Token: 0x04003E2B RID: 15915
			public Vector3 w3;
		}
	}
}
