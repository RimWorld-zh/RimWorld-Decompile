using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FAD RID: 4013
	[HasDebugOutput]
	public static class Rand
	{
		// Token: 0x060060DB RID: 24795 RVA: 0x0030E518 File Offset: 0x0030C918
		static Rand()
		{
			Rand.random.seed = (uint)DateTime.Now.GetHashCode();
		}

		// Token: 0x17000FA7 RID: 4007
		// (set) Token: 0x060060DC RID: 24796 RVA: 0x0030E567 File Offset: 0x0030C967
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

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x060060DD RID: 24797 RVA: 0x0030E59C File Offset: 0x0030C99C
		public static float Value
		{
			get
			{
				return Rand.random.GetFloat(Rand.iterations++);
			}
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x060060DE RID: 24798 RVA: 0x0030E5C8 File Offset: 0x0030C9C8
		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5f;
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x060060DF RID: 24799 RVA: 0x0030E5EC File Offset: 0x0030C9EC
		public static int Sign
		{
			get
			{
				return (!Rand.Bool) ? -1 : 1;
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x060060E0 RID: 24800 RVA: 0x0030E614 File Offset: 0x0030CA14
		public static int Int
		{
			get
			{
				return Rand.random.GetInt(Rand.iterations++);
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x060060E1 RID: 24801 RVA: 0x0030E640 File Offset: 0x0030CA40
		public static Vector3 UnitVector3
		{
			get
			{
				Vector3 vector = new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f));
				return vector.normalized;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x060060E2 RID: 24802 RVA: 0x0030E690 File Offset: 0x0030CA90
		public static Vector2 UnitVector2
		{
			get
			{
				Vector2 vector = new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f));
				return vector.normalized;
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x060060E3 RID: 24803 RVA: 0x0030E6D4 File Offset: 0x0030CAD4
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

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x060060E4 RID: 24804 RVA: 0x0030E72C File Offset: 0x0030CB2C
		public static Vector3 InsideUnitCircleVec3
		{
			get
			{
				Vector2 insideUnitCircle = Rand.InsideUnitCircle;
				return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x060060E5 RID: 24805 RVA: 0x0030E760 File Offset: 0x0030CB60
		// (set) Token: 0x060060E6 RID: 24806 RVA: 0x0030E78A File Offset: 0x0030CB8A
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

		// Token: 0x060060E7 RID: 24807 RVA: 0x0030E7A9 File Offset: 0x0030CBA9
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

		// Token: 0x060060E8 RID: 24808 RVA: 0x0030E7E4 File Offset: 0x0030CBE4
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.28318548f * value2);
			return num * widthFactor + centerX;
		}

		// Token: 0x060060E9 RID: 24809 RVA: 0x0030E82C File Offset: 0x0030CC2C
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

		// Token: 0x060060EA RID: 24810 RVA: 0x0030E888 File Offset: 0x0030CC88
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

		// Token: 0x060060EB RID: 24811 RVA: 0x0030E8D0 File Offset: 0x0030CCD0
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

		// Token: 0x060060EC RID: 24812 RVA: 0x0030E8FC File Offset: 0x0030CCFC
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

		// Token: 0x060060ED RID: 24813 RVA: 0x0030E92C File Offset: 0x0030CD2C
		public static bool Chance(float chance)
		{
			return chance > 0f && (chance >= 1f || Rand.Value < chance);
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x0030E970 File Offset: 0x0030CD70
		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			Rand.PushState(specialSeed);
			bool result = Rand.Chance(chance);
			Rand.PopState();
			return result;
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x0030E998 File Offset: 0x0030CD98
		public static float ValueSeeded(int specialSeed)
		{
			Rand.PushState(specialSeed);
			float value = Rand.Value;
			Rand.PopState();
			return value;
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x0030E9C0 File Offset: 0x0030CDC0
		public static float RangeSeeded(float min, float max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			float result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x0030E9EC File Offset: 0x0030CDEC
		public static int RangeSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x060060F2 RID: 24818 RVA: 0x0030EA18 File Offset: 0x0030CE18
		public static int RangeInclusiveSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.RangeInclusive(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x0030EA44 File Offset: 0x0030CE44
		public static T Element<T>(T a, T b)
		{
			return (!Rand.Bool) ? b : a;
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x0030EA6C File Offset: 0x0030CE6C
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

		// Token: 0x060060F5 RID: 24821 RVA: 0x0030EAAC File Offset: 0x0030CEAC
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

		// Token: 0x060060F6 RID: 24822 RVA: 0x0030EB00 File Offset: 0x0030CF00
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

		// Token: 0x060060F7 RID: 24823 RVA: 0x0030EB68 File Offset: 0x0030CF68
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

		// Token: 0x060060F8 RID: 24824 RVA: 0x0030EBE0 File Offset: 0x0030CFE0
		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x0030EBF2 File Offset: 0x0030CFF2
		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x0030EC00 File Offset: 0x0030D000
		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x0030EC14 File Offset: 0x0030D014
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

		// Token: 0x060060FC RID: 24828 RVA: 0x0030EE50 File Offset: 0x0030D250
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

		// Token: 0x060060FD RID: 24829 RVA: 0x0030EF80 File Offset: 0x0030D380
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

		// Token: 0x060060FE RID: 24830 RVA: 0x0030F100 File Offset: 0x0030D500
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

		// Token: 0x060060FF RID: 24831 RVA: 0x0030F4B0 File Offset: 0x0030D8B0
		public static int RandSeedForHour(this Thing t, int salt)
		{
			int seed = t.HashOffset();
			seed = Gen.HashCombineInt(seed, Find.TickManager.TicksAbs / 2500);
			return Gen.HashCombineInt(seed, salt);
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x0030F4EC File Offset: 0x0030D8EC
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

		// Token: 0x06006101 RID: 24833 RVA: 0x0030F5F0 File Offset: 0x0030D9F0
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

		// Token: 0x04003F6D RID: 16237
		private static Stack<ulong> stateStack = new Stack<ulong>();

		// Token: 0x04003F6E RID: 16238
		private static RandomNumberGenerator random = new RandomNumberGenerator_BasicHash();

		// Token: 0x04003F6F RID: 16239
		private static uint iterations = 0u;

		// Token: 0x04003F70 RID: 16240
		private static List<int> tmpRange = new List<int>();
	}
}
