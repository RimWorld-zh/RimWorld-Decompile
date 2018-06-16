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
	// Token: 0x02000F2E RID: 3886
	public static class Gen
	{
		// Token: 0x06005D37 RID: 23863 RVA: 0x002F3878 File Offset: 0x002F1C78
		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		// Token: 0x06005D38 RID: 23864 RVA: 0x002F38EC File Offset: 0x002F1CEC
		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int min = (!disallowFirstValue) ? 0 : 1;
			int num = Rand.Range(min, Enum.GetValues(typeof(T)).Length);
			return (T)((object)num);
		}

		// Token: 0x06005D39 RID: 23865 RVA: 0x002F3938 File Offset: 0x002F1D38
		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

		// Token: 0x06005D3A RID: 23866 RVA: 0x002F3968 File Offset: 0x002F1D68
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

		// Token: 0x06005D3B RID: 23867 RVA: 0x002F399C File Offset: 0x002F1D9C
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

		// Token: 0x06005D3C RID: 23868 RVA: 0x002F39C8 File Offset: 0x002F1DC8
		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

		// Token: 0x06005D3D RID: 23869 RVA: 0x002F39F4 File Offset: 0x002F1DF4
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

		// Token: 0x06005D3E RID: 23870 RVA: 0x002F3AB0 File Offset: 0x002F1EB0
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

		// Token: 0x06005D3F RID: 23871 RVA: 0x002F3BD0 File Offset: 0x002F1FD0
		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		// Token: 0x06005D40 RID: 23872 RVA: 0x002F3BF8 File Offset: 0x002F1FF8
		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		// Token: 0x06005D41 RID: 23873 RVA: 0x002F3C48 File Offset: 0x002F2048
		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		// Token: 0x06005D42 RID: 23874 RVA: 0x002F3C90 File Offset: 0x002F2090
		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj != null) ? obj.GetHashCode() : 0;
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x002F3CDC File Offset: 0x002F20DC
		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x002F3D14 File Offset: 0x002F2114
		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x002F3D40 File Offset: 0x002F2140
		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x002F3D60 File Offset: 0x002F2160
		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x002F3D80 File Offset: 0x002F2180
		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x002F3DA0 File Offset: 0x002F21A0
		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x002F3DC0 File Offset: 0x002F21C0
		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x002F3DEC File Offset: 0x002F21EC
		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x002F3E0C File Offset: 0x002F220C
		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x002F3E38 File Offset: 0x002F2238
		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x002F3E58 File Offset: 0x002F2258
		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x002F3E84 File Offset: 0x002F2284
		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x002F3EB8 File Offset: 0x002F22B8
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

		// Token: 0x06005D50 RID: 23888 RVA: 0x002F3F68 File Offset: 0x002F2368
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

		// Token: 0x06005D51 RID: 23889 RVA: 0x002F3FFC File Offset: 0x002F23FC
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

		// Token: 0x04003DAB RID: 15787
		private static MethodInfo s_memberwiseClone = null;
	}
}
