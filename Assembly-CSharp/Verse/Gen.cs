using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public static class Gen
	{
		private static MethodInfo s_memberwiseClone = null;

		[CompilerGenerated]
		private static Func<IntVec3, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IntVec3, int> <>f__am$cache1;

		public static Vector3 AveragePosition(List<IntVec3> cells)
		{
			return new Vector3((float)cells.Average((IntVec3 c) => c.x) + 0.5f, 0f, (float)cells.Average((IntVec3 c) => c.z) + 0.5f);
		}

		public static T RandomEnumValue<T>(bool disallowFirstValue)
		{
			int min = (!disallowFirstValue) ? 0 : 1;
			int num = Rand.Range(min, Enum.GetValues(typeof(T)).Length);
			return (T)((object)num);
		}

		public static Vector3 RandomHorizontalVector(float max)
		{
			return new Vector3(Rand.Range(-max, max), 0f, Rand.Range(-max, max));
		}

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

		public static IEnumerable<T> YieldSingle<T>(T val)
		{
			yield return val;
			yield break;
		}

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

		public static void Swap<T>(ref T x, ref T y)
		{
			T t = y;
			y = x;
			x = t;
		}

		public static T MemberwiseClone<T>(T obj)
		{
			if (Gen.s_memberwiseClone == null)
			{
				Gen.s_memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			}
			return (T)((object)Gen.s_memberwiseClone.Invoke(obj, null));
		}

		public static int FixedTimeStepUpdate(ref float timeBuffer, float fps)
		{
			timeBuffer += Mathf.Min(Time.deltaTime, 1f);
			float num = 1f / fps;
			int num2 = Mathf.FloorToInt(timeBuffer / num);
			timeBuffer -= (float)num2 * num;
			return num2;
		}

		public static int HashCombine<T>(int seed, T obj)
		{
			int num = (obj != null) ? obj.GetHashCode() : 0;
			return (int)((long)seed ^ (long)num + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		public static int HashCombineStruct<T>(int seed, T obj) where T : struct
		{
			return (int)((long)seed ^ (long)obj.GetHashCode() + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		public static int HashCombineInt(int seed, int value)
		{
			return (int)((long)seed ^ (long)value + (long)((ulong)-1640531527) + (long)((long)seed << 6) + (long)(seed >> 2));
		}

		public static int HashOffset(this int baseInt)
		{
			return Gen.HashCombineInt(baseInt, 169495093);
		}

		public static int HashOffset(this Thing t)
		{
			return t.thingIDNumber.HashOffset();
		}

		public static int HashOffset(this WorldObject o)
		{
			return o.ID.HashOffset();
		}

		public static bool IsHashIntervalTick(this Thing t, int interval)
		{
			return t.HashOffsetTicks() % interval == 0;
		}

		public static int HashOffsetTicks(this Thing t)
		{
			return Find.TickManager.TicksGame + t.thingIDNumber.HashOffset();
		}

		public static bool IsHashIntervalTick(this WorldObject o, int interval)
		{
			return o.HashOffsetTicks() % interval == 0;
		}

		public static int HashOffsetTicks(this WorldObject o)
		{
			return Find.TickManager.TicksGame + o.ID.HashOffset();
		}

		public static bool IsHashIntervalTick(this Faction f, int interval)
		{
			return f.HashOffsetTicks() % interval == 0;
		}

		public static int HashOffsetTicks(this Faction f)
		{
			return Find.TickManager.TicksGame + f.randomKey.HashOffset();
		}

		public static bool IsNestedHashIntervalTick(this Thing t, int outerInterval, int approxInnerInterval)
		{
			int num = Mathf.Max(Mathf.RoundToInt((float)approxInnerInterval / (float)outerInterval), 1);
			return t.HashOffsetTicks() / outerInterval % num == 0;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Gen()
		{
		}

		[CompilerGenerated]
		private static int <AveragePosition>m__0(IntVec3 c)
		{
			return c.x;
		}

		[CompilerGenerated]
		private static int <AveragePosition>m__1(IntVec3 c)
		{
			return c.z;
		}

		[CompilerGenerated]
		private sealed class <GetAllSelectedItems>c__Iterator0<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal Enum value;

			internal int <valueAsInt>__0;

			internal IEnumerator $locvar0;

			internal object <item>__1;

			internal IDisposable $locvar1;

			internal int <itemAsInt>__2;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetAllSelectedItems>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					valueAsInt = Convert.ToInt32(value);
					enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					IL_C8:
					if (enumerator.MoveNext())
					{
						item = enumerator.Current;
						itemAsInt = Convert.ToInt32(item);
						if (itemAsInt == (valueAsInt & itemAsInt))
						{
							this.$current = (T)((object)item);
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_C8;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Gen.<GetAllSelectedItems>c__Iterator0<T> <GetAllSelectedItems>c__Iterator = new Gen.<GetAllSelectedItems>c__Iterator0<T>();
				<GetAllSelectedItems>c__Iterator.value = value;
				return <GetAllSelectedItems>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <YieldSingle>c__Iterator1<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
		{
			internal T val;

			internal T $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <YieldSingle>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = val;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Gen.<YieldSingle>c__Iterator1<T> <YieldSingle>c__Iterator = new Gen.<YieldSingle>c__Iterator1<T>();
				<YieldSingle>c__Iterator.val = val;
				return <YieldSingle>c__Iterator;
			}
		}
	}
}
