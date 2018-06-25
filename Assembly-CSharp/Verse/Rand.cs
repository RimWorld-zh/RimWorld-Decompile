using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FB0 RID: 4016
	[HasDebugOutput]
	public static class Rand
	{
		// Token: 0x04003F81 RID: 16257
		private static Stack<ulong> stateStack = new Stack<ulong>();

		// Token: 0x04003F82 RID: 16258
		private static RandomNumberGenerator random = new RandomNumberGenerator_BasicHash();

		// Token: 0x04003F83 RID: 16259
		private static uint iterations = 0u;

		// Token: 0x04003F84 RID: 16260
		private static List<int> tmpRange = new List<int>();

		// Token: 0x0600610C RID: 24844 RVA: 0x00310D18 File Offset: 0x0030F118
		static Rand()
		{
			Rand.random.seed = (uint)DateTime.Now.GetHashCode();
		}

		// Token: 0x17000FA9 RID: 4009
		// (set) Token: 0x0600610D RID: 24845 RVA: 0x00310D67 File Offset: 0x0030F167
		public static int Seed
		{
			set
			{
				if (Rand.stateStack.Count == 0)
				{
					Log.ErrorOnce("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.", 825343540, false);
				}
				Rand.random.seed = (uint)value;
				Rand.iterations = 0u;
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x0600610E RID: 24846 RVA: 0x00310D9C File Offset: 0x0030F19C
		public static float Value
		{
			get
			{
				return Rand.random.GetFloat(Rand.iterations++);
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x0600610F RID: 24847 RVA: 0x00310DC8 File Offset: 0x0030F1C8
		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5f;
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06006110 RID: 24848 RVA: 0x00310DEC File Offset: 0x0030F1EC
		public static int Sign
		{
			get
			{
				return (!Rand.Bool) ? -1 : 1;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06006111 RID: 24849 RVA: 0x00310E14 File Offset: 0x0030F214
		public static int Int
		{
			get
			{
				return Rand.random.GetInt(Rand.iterations++);
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06006112 RID: 24850 RVA: 0x00310E40 File Offset: 0x0030F240
		public static Vector3 UnitVector3
		{
			get
			{
				Vector3 vector = new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f));
				return vector.normalized;
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06006113 RID: 24851 RVA: 0x00310E90 File Offset: 0x0030F290
		public static Vector2 UnitVector2
		{
			get
			{
				Vector2 vector = new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f));
				return vector.normalized;
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06006114 RID: 24852 RVA: 0x00310ED4 File Offset: 0x0030F2D4
		public static Vector2 InsideUnitCircle
		{
			get
			{
				Vector2 result;
				do
				{
					result = new Vector2(Rand.Value - 0.5f, Rand.Value - 0.5f) * 2f;
				}
				while (result.sqrMagnitude > 1f);
				return result;
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06006115 RID: 24853 RVA: 0x00310F2C File Offset: 0x0030F32C
		public static Vector3 InsideUnitCircleVec3
		{
			get
			{
				Vector2 insideUnitCircle = Rand.InsideUnitCircle;
				return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06006116 RID: 24854 RVA: 0x00310F60 File Offset: 0x0030F360
		// (set) Token: 0x06006117 RID: 24855 RVA: 0x00310F8A File Offset: 0x0030F38A
		private static ulong StateCompressed
		{
			get
			{
				return (ulong)Rand.random.seed | (ulong)Rand.iterations << 32;
			}
			set
			{
				Rand.random.seed = (uint)(value & (ulong)-1);
				Rand.iterations = (uint)(value >> 32 & (ulong)-1);
			}
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x00310FA9 File Offset: 0x0030F3A9
		public static void EnsureStateStackEmpty()
		{
			if (Rand.stateStack.Count > 0)
			{
				Log.Warning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.", false);
				while (Rand.stateStack.Any<ulong>())
				{
					Rand.PopState();
				}
			}
		}

		// Token: 0x06006119 RID: 24857 RVA: 0x00310FE4 File Offset: 0x0030F3E4
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.28318548f * value2);
			return num * widthFactor + centerX;
		}

		// Token: 0x0600611A RID: 24858 RVA: 0x0031102C File Offset: 0x0030F42C
		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.28318548f * value2);
			float result;
			if (num <= 0f)
			{
				result = num * lowerWidthFactor + centerX;
			}
			else
			{
				result = num * upperWidthFactor + centerX;
			}
			return result;
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x00311088 File Offset: 0x0030F488
		public static int Range(int min, int max)
		{
			int result;
			if (max <= min)
			{
				result = min;
			}
			else
			{
				result = min + Mathf.Abs(Rand.random.GetInt(Rand.iterations++) % (max - min));
			}
			return result;
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x003110D0 File Offset: 0x0030F4D0
		public static int RangeInclusive(int min, int max)
		{
			int result;
			if (max <= min)
			{
				result = min;
			}
			else
			{
				result = Rand.Range(min, max + 1);
			}
			return result;
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x003110FC File Offset: 0x0030F4FC
		public static float Range(float min, float max)
		{
			float result;
			if (max <= min)
			{
				result = min;
			}
			else
			{
				result = Rand.Value * (max - min) + min;
			}
			return result;
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x0031112C File Offset: 0x0030F52C
		public static bool Chance(float chance)
		{
			return chance > 0f && (chance >= 1f || Rand.Value < chance);
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x00311170 File Offset: 0x0030F570
		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			Rand.PushState(specialSeed);
			bool result = Rand.Chance(chance);
			Rand.PopState();
			return result;
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x00311198 File Offset: 0x0030F598
		public static float ValueSeeded(int specialSeed)
		{
			Rand.PushState(specialSeed);
			float value = Rand.Value;
			Rand.PopState();
			return value;
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x003111C0 File Offset: 0x0030F5C0
		public static float RangeSeeded(float min, float max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			float result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x06006122 RID: 24866 RVA: 0x003111EC File Offset: 0x0030F5EC
		public static int RangeSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x00311218 File Offset: 0x0030F618
		public static int RangeInclusiveSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.RangeInclusive(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x06006124 RID: 24868 RVA: 0x00311244 File Offset: 0x0030F644
		public static T Element<T>(T a, T b)
		{
			return (!Rand.Bool) ? b : a;
		}

		// Token: 0x06006125 RID: 24869 RVA: 0x0031126C File Offset: 0x0030F66C
		public static T Element<T>(T a, T b, T c)
		{
			float value = Rand.Value;
			T result;
			if (value < 0.33333f)
			{
				result = a;
			}
			else if (value < 0.66666f)
			{
				result = b;
			}
			else
			{
				result = c;
			}
			return result;
		}

		// Token: 0x06006126 RID: 24870 RVA: 0x003112AC File Offset: 0x0030F6AC
		public static T Element<T>(T a, T b, T c, T d)
		{
			float value = Rand.Value;
			T result;
			if (value < 0.25f)
			{
				result = a;
			}
			else if (value < 0.5f)
			{
				result = b;
			}
			else if (value < 0.75f)
			{
				result = c;
			}
			else
			{
				result = d;
			}
			return result;
		}

		// Token: 0x06006127 RID: 24871 RVA: 0x00311300 File Offset: 0x0030F700
		public static T Element<T>(T a, T b, T c, T d, T e)
		{
			float value = Rand.Value;
			T result;
			if (value < 0.2f)
			{
				result = a;
			}
			else if (value < 0.4f)
			{
				result = b;
			}
			else if (value < 0.6f)
			{
				result = c;
			}
			else if (value < 0.8f)
			{
				result = d;
			}
			else
			{
				result = e;
			}
			return result;
		}

		// Token: 0x06006128 RID: 24872 RVA: 0x00311368 File Offset: 0x0030F768
		public static T Element<T>(T a, T b, T c, T d, T e, T f)
		{
			float value = Rand.Value;
			T result;
			if (value < 0.16666f)
			{
				result = a;
			}
			else if (value < 0.33333f)
			{
				result = b;
			}
			else if (value < 0.5f)
			{
				result = c;
			}
			else if (value < 0.66666f)
			{
				result = d;
			}
			else if (value < 0.83333f)
			{
				result = e;
			}
			else
			{
				result = f;
			}
			return result;
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x003113E0 File Offset: 0x0030F7E0
		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		// Token: 0x0600612A RID: 24874 RVA: 0x003113F2 File Offset: 0x0030F7F2
		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		// Token: 0x0600612B RID: 24875 RVA: 0x00311400 File Offset: 0x0030F800
		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		// Token: 0x0600612C RID: 24876 RVA: 0x00311414 File Offset: 0x0030F814
		public static float ByCurve(SimpleCurve curve)
		{
			if (curve.PointsCount < 3)
			{
				throw new ArgumentException("curve has < 3 points");
			}
			if (curve[0].y != 0f || curve[curve.PointsCount - 1].y != 0f)
			{
				throw new ArgumentException("curve has start/end point with y != 0");
			}
			float num = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				if (curve[i].y < 0f)
				{
					throw new ArgumentException("curve has point with y < 0");
				}
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
			}
			float num2 = Rand.Range(0f, num);
			for (int j = 0; j < curve.PointsCount - 1; j++)
			{
				float num3 = (curve[j + 1].x - curve[j].x) * (curve[j].y + curve[j + 1].y);
				if (num3 >= num2)
				{
					float num4 = curve[j + 1].x - curve[j].x;
					float y = curve[j].y;
					float y2 = curve[j + 1].y;
					float num5 = num2 / (y + y2);
					float num6 = Rand.Range(0f, (y + y2) / 2f);
					if (num6 > Mathf.Lerp(y, y2, num5 / num4))
					{
						num5 = num4 - num5;
					}
					return num5 + curve[j].x;
				}
				num2 -= num3;
			}
			throw new Exception("Reached end of Rand.ByCurve without choosing a point.");
		}

		// Token: 0x0600612D RID: 24877 RVA: 0x00311650 File Offset: 0x0030FA50
		public static float ByCurveAverage(SimpleCurve curve)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
				num2 += (curve[i + 1].x - curve[i].x) * (curve[i].x * (2f * curve[i].y + curve[i + 1].y) + curve[i + 1].x * (curve[i].y + 2f * curve[i + 1].y));
			}
			return num2 / num / 3f;
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x00311780 File Offset: 0x0030FB80
		public static bool MTBEventOccurs(float mtb, float mtbUnit, float checkDuration)
		{
			bool result;
			if (mtb == float.PositiveInfinity)
			{
				result = false;
			}
			else if (mtb <= 0f)
			{
				Log.Error("MTBEventOccurs with mtb=" + mtb, false);
				result = true;
			}
			else if (mtbUnit <= 0f)
			{
				Log.Error("MTBEventOccurs with mtbUnit=" + mtbUnit, false);
				result = false;
			}
			else if (checkDuration <= 0f)
			{
				Log.Error("MTBEventOccurs with checkDuration=" + checkDuration, false);
				result = false;
			}
			else
			{
				double num = (double)checkDuration / ((double)mtb * (double)mtbUnit);
				if (num <= 0.0)
				{
					Log.Error(string.Concat(new object[]
					{
						"chancePerCheck is ",
						num,
						". mtb=",
						mtb,
						", mtbUnit=",
						mtbUnit,
						", checkDuration=",
						checkDuration
					}), false);
					result = false;
				}
				else
				{
					double num2 = 1.0;
					if (num < 0.0001)
					{
						while (num < 0.0001)
						{
							num *= 8.0;
							num2 /= 8.0;
						}
						if ((double)Rand.Value > num2)
						{
							return false;
						}
					}
					result = ((double)Rand.Value < num);
				}
			}
			return result;
		}

		// Token: 0x0600612F RID: 24879 RVA: 0x00311900 File Offset: 0x0030FD00
		[DebugOutput]
		[Category("System")]
		internal static void RandTests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int @int = Rand.Int;
			stringBuilder.AppendLine("Repeating single ValueSeeded with seed " + @int + ". This should give the same result:");
			for (int i = 0; i < 4; i++)
			{
				stringBuilder.AppendLine("   " + Rand.ValueSeeded(@int));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Long-term tests");
			for (int j = 0; j < 3; j++)
			{
				int num = 0;
				for (int k = 0; k < 5000000; k++)
				{
					if (Rand.MTBEventOccurs(250f, 60000f, 60f))
					{
						num++;
					}
				}
				string value = string.Concat(new object[]
				{
					"MTB=",
					250,
					" days, MTBUnit=",
					60000,
					", check duration=",
					60,
					" Simulated ",
					5000,
					" days (",
					5000000,
					" tests). Got ",
					num,
					" events."
				});
				stringBuilder.AppendLine(value);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Short-term tests");
			for (int l = 0; l < 5; l++)
			{
				int num2 = 0;
				for (int m = 0; m < 10000; m++)
				{
					if (Rand.MTBEventOccurs(1f, 24000f, 12000f))
					{
						num2++;
					}
				}
				string value2 = string.Concat(new object[]
				{
					"MTB=",
					1f,
					" days, MTBUnit=",
					24000f,
					", check duration=",
					12000f,
					", ",
					10000,
					" tests got ",
					num2,
					" events."
				});
				stringBuilder.AppendLine(value2);
			}
			for (int n = 0; n < 5; n++)
			{
				int num3 = 0;
				for (int num4 = 0; num4 < 10000; num4++)
				{
					if (Rand.MTBEventOccurs(2f, 24000f, 6000f))
					{
						num3++;
					}
				}
				string value3 = string.Concat(new object[]
				{
					"MTB=",
					2f,
					" days, MTBUnit=",
					24000f,
					", check duration=",
					6000f,
					", ",
					10000,
					" tests got ",
					num3,
					" events."
				});
				stringBuilder.AppendLine(value3);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Near seed tests");
			DebugHistogram debugHistogram = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			Rand.PushState();
			for (int num5 = 0; num5 < 1000; num5++)
			{
				Rand.Seed = num5;
				debugHistogram.Add(Rand.Value);
			}
			Rand.PopState();
			debugHistogram.Display(stringBuilder);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x00311CB0 File Offset: 0x003100B0
		public static int RandSeedForHour(this Thing t, int salt)
		{
			int seed = t.HashOffset();
			seed = Gen.HashCombineInt(seed, Find.TickManager.TicksAbs / 2500);
			return Gen.HashCombineInt(seed, salt);
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x00311CEC File Offset: 0x003100EC
		public static bool TryRangeInclusiveWhere(int from, int to, Predicate<int> predicate, out int value)
		{
			int num = to - from + 1;
			bool result;
			if (num <= 0)
			{
				value = 0;
				result = false;
			}
			else
			{
				int num2 = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 5);
				for (int i = 0; i < num2; i++)
				{
					int num3 = Rand.RangeInclusive(from, to);
					if (predicate(num3))
					{
						value = num3;
						return true;
					}
				}
				Rand.tmpRange.Clear();
				for (int j = from; j <= to; j++)
				{
					Rand.tmpRange.Add(j);
				}
				Rand.tmpRange.Shuffle<int>();
				int k = 0;
				int count = Rand.tmpRange.Count;
				while (k < count)
				{
					if (predicate(Rand.tmpRange[k]))
					{
						value = Rand.tmpRange[k];
						return true;
					}
					k++;
				}
				value = 0;
				result = false;
			}
			return result;
		}

		// Token: 0x06006132 RID: 24882 RVA: 0x00311DF0 File Offset: 0x003101F0
		public static Vector3 PointOnSphereCap(Vector3 center, float angle)
		{
			Vector3 result;
			if (angle <= 0f)
			{
				result = center;
			}
			else if (angle >= 180f)
			{
				result = Rand.UnitVector3;
			}
			else
			{
				float num = Rand.Range(Mathf.Cos(angle * 0.0174532924f), 1f);
				float f = Rand.Range(0f, 6.28318548f);
				Vector3 point = new Vector3(Mathf.Sqrt(1f - num * num) * Mathf.Cos(f), Mathf.Sqrt(1f - num * num) * Mathf.Sin(f), num);
				result = Quaternion.FromToRotation(Vector3.forward, center) * point;
			}
			return result;
		}
	}
}
