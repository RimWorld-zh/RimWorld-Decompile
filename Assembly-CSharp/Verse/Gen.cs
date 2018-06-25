using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F31 RID: 3889
	public static class Gen
	{
		// Token: 0x04003DBF RID: 15807
		private static MethodInfo s_memberwiseClone = null;

		// Token: 0x06005D67 RID: 23911 RVA: 0x002F6000 File Offset: 0x002F4400
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x002F6074 File Offset: 0x002F4474
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int min = (!disallowFirstValue) ? 0 : 1;
			int num = Rand.Range(min, Enum.GetValues(typeof(T)).Length);
			return (T)((object)num);
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x002F60C0 File Offset: 0x002F44C0
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x002F60F0 File Offset: 0x002F44F0
		public static int GetBitCountOf(long lValue)
		{
			int num = 0;
			while (lValue != 0L)
			{
				lValue &= lValue - 1L;
				num++;
			}
			return num;
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x002F6124 File Offset: 0x002F4524
		public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
		{
			int valueAsInt = Convert.ToInt32(value);
			IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object item = enumerator.Current;
					int itemAsInt = Convert.ToInt32(item);
					if (itemAsInt == (valueAsInt & itemAsInt))
					{
						yield return (T)((object)item);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			yield break;
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x002F6150 File Offset: 0x002F4550
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x002F617C File Offset: 0x002F457C
		public static string ToStringSafe<T>(this T obj)
		{
			string result;
			if (obj == null)
			{
				result = "null";
			}
			else
			{
				try
				{
					result = obj.ToString();
				}
				catch (Exception arg)
				{
					int num = 0;
					bool flag = false;
					try
					{
						num = obj.GetHashCode();
						flag = true;
					}
					catch
					{
					}
					if (flag)
					{
						Log.ErrorOnce("Exception in ToString(): " + arg, num ^ 1857461521, false);
					}
					else
					{
						Log.Error("Exception in ToString(): " + arg, false);
					}
					result = "error";
				}
			}
			return result;
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x002F6238 File Offset: 0x002F4638
		public static string ToStringSafeEnumerable(this IEnumerable enumerable)
		{
			string result;
			if (enumerable == null)
			{
				result = "null";
			}
			else
			{
				try
				{
					string text = "";
					IEnumerator enumerator = enumerable.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							if (text.Length > 0)
							{
								text += ", ";
							}
							text += obj.ToStringSafe<object>();
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					result = text;
				}
				catch (Exception arg)
				{
					int num = 0;
					bool flag = false;
					try
					{
						num = enumerable.GetHashCode();
						flag = true;
					}
					catch
					{
					}
					if (flag)
					{
						Log.ErrorOnce("Exception while enumerating: " + arg, num ^ 581736153, false);
					}
					else
					{
						Log.Error("Exception while enumerating: " + arg, false);
					}
					result = "error";
				}
			}
			return result;
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x002F6358 File Offset: 0x002F4758
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x002F6380 File Offset: 0x002F4780
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x002F63D0 File Offset: 0x002F47D0
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x002F6418 File Offset: 0x002F4818
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj != null) ? obj.GetHashCode() : 0;
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x002F6464 File Offset: 0x002F4864
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x002F649C File Offset: 0x002F489C
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06005D75 RID: 23925 RVA: 0x002F64C8 File Offset: 0x002F48C8
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x06005D76 RID: 23926 RVA: 0x002F64E8 File Offset: 0x002F48E8
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x06005D77 RID: 23927 RVA: 0x002F6508 File Offset: 0x002F4908
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x06005D78 RID: 23928 RVA: 0x002F6528 File Offset: 0x002F4928
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x002F6548 File Offset: 0x002F4948
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x002F6574 File Offset: 0x002F4974
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x002F6594 File Offset: 0x002F4994
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x06005D7C RID: 23932 RVA: 0x002F65C0 File Offset: 0x002F49C0
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x002F65E0 File Offset: 0x002F49E0
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x002F660C File Offset: 0x002F4A0C
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x002F6640 File Offset: 0x002F4A40
		public static void ReplaceNullFields<T>(ref T replaceIn, T replaceWith)
		{
			if (replaceIn != null && replaceWith != null)
			{
				foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (fieldInfo.GetValue(replaceIn) == null)
					{
						object value = fieldInfo.GetValue(replaceWith);
						if (value != null)
						{
							object obj = replaceIn;
							fieldInfo.SetValue(obj, value);
							replaceIn = (T)((object)obj);
						}
					}
				}
			}
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x002F66F0 File Offset: 0x002F4AF0
		public static void EnsureAllFieldsNullable(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsValueType)
				{
					if (Nullable.GetUnderlyingType(fieldType) == null)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Field ",
							type.Name,
							".",
							fieldInfo.Name,
							" is not nullable."
						}), false);
					}
				}
			}
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x002F6784 File Offset: 0x002F4B84
		public static string GetNonNullFieldsDebugInfo(object obj)
		{
			string result;
			if (obj == null)
			{
				result = "";
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					object value = fieldInfo.GetValue(obj);
					if (value != null)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(fieldInfo.Name + "=" + value.ToStringSafe<object>());
					}
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
	}
}
