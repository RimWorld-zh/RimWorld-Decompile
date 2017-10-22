using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	public static class Rand
	{
		private static Stack<ulong> stateStack = new Stack<ulong>();

		private static RandomNumberGenerator random = new RandomNumberGenerator_BasicHash();

		private static uint iterations = 0u;

		private static List<int> tmpRange = new List<int>();

		public static int Seed
		{
			set
			{
				Rand.random.seed = (uint)value;
				Rand.iterations = 0u;
			}
		}

		public static float Value
		{
			get
			{
				return Rand.random.GetFloat(Rand.iterations++);
			}
		}

		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5;
			}
		}

		public static int Int
		{
			get
			{
				return Rand.random.GetInt(Rand.iterations++);
			}
		}

		public static Vector3 UnitVector3
		{
			get
			{
				return new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		public static Vector2 UnitVector2
		{
			get
			{
				return new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		public static Vector3 PointOnDisc
		{
			get
			{
				Vector3 result;
				while (true)
				{
					result = new Vector3((float)(Rand.Value - 0.5), 0f, (float)(Rand.Value - 0.5)) * 2f;
					if (result.sqrMagnitude <= 1.0)
						break;
				}
				return result;
			}
		}

		private static ulong StateCompressed
		{
			get
			{
				return Rand.random.seed | (ulong)Rand.iterations << 32;
			}
			set
			{
				Rand.random.seed = (uint)(value & 4294967295u);
				Rand.iterations = (uint)(value >> 32 & 4294967295u);
			}
		}

		public static void EnsureStateStackEmpty()
		{
			if (Rand.stateStack.Any())
			{
				Log.Warning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.");
				Rand.stateStack.Clear();
			}
		}

		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt((float)(-2.0 * Mathf.Log(value))) * Mathf.Sin((float)(6.2831854820251465 * value2));
			return num * widthFactor + centerX;
		}

		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt((float)(-2.0 * Mathf.Log(value))) * Mathf.Sin((float)(6.2831854820251465 * value2));
			return (!(num <= 0.0)) ? (num * upperWidthFactor + centerX) : (num * lowerWidthFactor + centerX);
		}

		public static void RandomizeStateFromTime()
		{
			Rand.Seed = DateTime.Now.GetHashCode();
		}

		public static int Range(int min, int max)
		{
			return (max > min) ? (min + Mathf.Abs(Rand.random.GetInt(Rand.iterations++) % (max - min))) : min;
		}

		public static int RangeInclusive(int min, int max)
		{
			return (max > min) ? Rand.Range(min, max + 1) : min;
		}

		public static float Range(float min, float max)
		{
			return (!(max <= min)) ? (Rand.Value * (max - min) + min) : min;
		}

		public static bool Chance(float chance)
		{
			return !(chance <= 0.0) && (chance >= 1.0 || Rand.Value < chance);
		}

		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			ulong stateCompressed = Rand.StateCompressed;
			Rand.Seed = specialSeed;
			bool result = Rand.Chance(chance);
			Rand.StateCompressed = stateCompressed;
			return result;
		}

		public static float ValueSeeded(int specialSeed)
		{
			ulong stateCompressed = Rand.StateCompressed;
			Rand.Seed = specialSeed;
			float value = Rand.Value;
			Rand.StateCompressed = stateCompressed;
			return value;
		}

		public static T Element<T>(T a, T b)
		{
			return (!Rand.Bool) ? b : a;
		}

		public static T Element<T>(T a, T b, T c)
		{
			float value = Rand.Value;
			return (!(value < 0.33333000540733337)) ? ((!(value < 0.66666001081466675)) ? c : b) : a;
		}

		public static T Element<T>(T a, T b, T c, T d)
		{
			float value = Rand.Value;
			return (!(value < 0.25)) ? ((!(value < 0.5)) ? ((!(value < 0.75)) ? d : c) : b) : a;
		}

		public static T Element<T>(T a, T b, T c, T d, T e)
		{
			float value = Rand.Value;
			return (!(value < 0.20000000298023224)) ? ((!(value < 0.40000000596046448)) ? ((!(value < 0.60000002384185791)) ? ((!(value < 0.800000011920929)) ? e : d) : c) : b) : a;
		}

		public static T Element<T>(T a, T b, T c, T d, T e, T f)
		{
			float value = Rand.Value;
			return (!(value < 0.16665999591350555)) ? ((!(value < 0.33333000540733337)) ? ((!(value < 0.5)) ? ((!(value < 0.66666001081466675)) ? ((!(value < 0.833329975605011)) ? f : e) : d) : c) : b) : a;
		}

		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		public static float ByCurve(SimpleCurve curve, int sampleCount = 100)
		{
			if (curve.PointsCount < 3)
			{
				throw new ArgumentException("curve has < 3 points");
			}
			if (!(curve[0].y > 0.0) && !(curve[curve.PointsCount - 1].y > 0.0))
			{
				float x = curve[0].x;
				float x2 = curve[curve.PointsCount - 1].x;
				float num = (x2 - x) / (float)sampleCount;
				float num2 = 0f;
				for (int num3 = 0; num3 < sampleCount; num3++)
				{
					float x3 = (float)(x + ((float)num3 + 0.5) * num);
					float num4 = curve.Evaluate(x3);
					num2 += num4;
				}
				float num5 = Rand.Range(0f, num2);
				num2 = 0f;
				for (int num6 = 0; num6 < sampleCount; num6++)
				{
					float num7 = (float)(x + ((float)num6 + 0.5) * num);
					float num8 = curve.Evaluate(num7);
					num2 += num8;
					if (num2 > num5)
					{
						return num7 + Rand.Range((float)((0.0 - num) / 2.0), (float)(num / 2.0));
					}
				}
				throw new Exception("Reached end of Rand.ByCurve without choosing a point.");
			}
			throw new ArgumentException("curve has start/end point with y > 0");
		}

		public static bool MTBEventOccurs(float mtb, float mtbUnit, float checkDuration)
		{
			bool result;
			if (mtb == double.PositiveInfinity)
			{
				result = false;
			}
			else if (mtb <= 0.0)
			{
				Log.Error("MTBEventOccurs with mtb=" + mtb);
				result = true;
			}
			else if (mtbUnit <= 0.0)
			{
				Log.Error("MTBEventOccurs with mtbUnit=" + mtbUnit);
				result = false;
			}
			else if (checkDuration <= 0.0)
			{
				Log.Error("MTBEventOccurs with checkDuration=" + checkDuration);
				result = false;
			}
			else
			{
				double num = (double)checkDuration / ((double)mtb * (double)mtbUnit);
				if (num <= 0.0)
				{
					Log.Error("chancePerCheck is " + num + ". mtb=" + mtb + ", mtbUnit=" + mtbUnit + ", checkDuration=" + checkDuration);
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
							result = false;
							goto IL_016b;
						}
					}
					result = ((double)Rand.Value < num);
				}
			}
			goto IL_016b;
			IL_016b:
			return result;
		}

		internal static void LogRandTests()
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
				string value = "MTB=" + 250 + " days, MTBUnit=" + 60000 + ", check duration=" + 60 + " Simulated " + 5000 + " days (" + 5000000 + " tests). Got " + num + " events.";
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
				string value2 = "MTB=" + 1f + " days, MTBUnit=" + 24000f + ", check duration=" + 12000f + ", " + 10000 + " tests got " + num2 + " events.";
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
				string value3 = "MTB=" + 2f + " days, MTBUnit=" + 24000f + ", check duration=" + 6000f + ", " + 10000 + " tests got " + num3 + " events.";
				stringBuilder.AppendLine(value3);
			}
			Log.Message(stringBuilder.ToString());
		}

		public static int RandSeedForHour(this Thing t, int salt)
		{
			int seed = t.HashOffset();
			seed = Gen.HashCombineInt(seed, Find.TickManager.TicksAbs / 2500);
			return Gen.HashCombineInt(seed, salt);
		}

		public static bool TryRangeInclusiveWhere(int from, int to, Predicate<int> predicate, out int value)
		{
			int num = to - from + 1;
			int num2 = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 5);
			int num3 = 0;
			bool result;
			while (true)
			{
				if (num3 < num2)
				{
					int num4 = Rand.RangeInclusive(from, to);
					if (predicate(num4))
					{
						value = num4;
						result = true;
						break;
					}
					num3++;
					continue;
				}
				Rand.tmpRange.Clear();
				for (int num5 = from; num5 <= to; num5++)
				{
					Rand.tmpRange.Add(num5);
				}
				Rand.tmpRange.Shuffle();
				int num6 = 0;
				int count = Rand.tmpRange.Count;
				while (num6 < count)
				{
					if (predicate(Rand.tmpRange[num6]))
						goto IL_00b2;
					num6++;
				}
				value = 0;
				result = false;
				break;
				IL_00b2:
				value = Rand.tmpRange[num6];
				result = true;
				break;
			}
			return result;
		}

		public static Vector3 PointOnSphereCap(Vector3 center, float angle)
		{
			Vector3 result;
			if (angle <= 0.0)
			{
				result = center;
			}
			else if (angle >= 180.0)
			{
				result = Rand.UnitVector3;
			}
			else
			{
				float num = Rand.Range(Mathf.Cos((float)(angle * 0.01745329238474369)), 1f);
				float f = Rand.Range(0f, 6.28318548f);
				Vector3 point = new Vector3(Mathf.Sqrt((float)(1.0 - num * num)) * Mathf.Cos(f), Mathf.Sqrt((float)(1.0 - num * num)) * Mathf.Sin(f), num);
				result = Quaternion.FromToRotation(Vector3.forward, center) * point;
			}
			return result;
		}
	}
}
