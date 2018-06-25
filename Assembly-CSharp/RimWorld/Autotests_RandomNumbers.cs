using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public static class Autotests_RandomNumbers
	{
		[CompilerGenerated]
		private static Predicate<float> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<float> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<float> <>f__am$cache2;

		[CompilerGenerated]
		private static Predicate<float> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<float, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<float, bool> <>f__am$cache5;

		public static void Run()
		{
			Log.Message("Running random numbers tests.", false);
			Autotests_RandomNumbers.CheckSimpleFloats();
			Autotests_RandomNumbers.CheckIntsRange();
			Autotests_RandomNumbers.CheckIntsDistribution();
			Autotests_RandomNumbers.CheckSeed();
			Log.Message("Finished.", false);
		}

		private static void CheckSimpleFloats()
		{
			List<float> list = Autotests_RandomNumbers.RandomFloats(500).ToList<float>();
			if (list.Any((float x) => x < 0f || x > 1f))
			{
				Log.Error("Float out of range.", false);
			}
			if (list.Any((float x) => x < 0.1f))
			{
				if (list.Any((float x) => (double)x > 0.5 && (double)x < 0.6))
				{
					if (list.Any((float x) => (double)x > 0.9))
					{
						goto IL_C7;
					}
				}
			}
			Log.Warning("Possibly uneven distribution.", false);
			IL_C7:
			list = Autotests_RandomNumbers.RandomFloats(1300000).ToList<float>();
			int num = list.Count((float x) => (double)x < 0.1);
			Log.Message("< 0.1 count (should be ~10%): " + (float)num / (float)list.Count<float>() * 100f + "%", false);
			num = list.Count((float x) => (double)x < 0.0001);
			Log.Message("< 0.0001 count (should be ~0.01%): " + (float)num / (float)list.Count<float>() * 100f + "%", false);
		}

		private static IEnumerable<float> RandomFloats(int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return Rand.Value;
			}
			yield break;
		}

		private static void CheckIntsRange()
		{
			int num = -7;
			int num2 = 4;
			int num3 = 0;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (;;)
			{
				bool flag = true;
				for (int i = num; i <= num2; i++)
				{
					if (!dictionary.ContainsKey(i))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num3++;
				if (num3 == 200000)
				{
					goto Block_3;
				}
				int num4 = Rand.RangeInclusive(num, num2);
				if (num4 < num || num4 > num2)
				{
					Log.Error("Value out of range.", false);
				}
				if (dictionary.ContainsKey(num4))
				{
					Dictionary<int, int> dictionary2;
					int key;
					(dictionary2 = dictionary)[key = num4] = dictionary2[key] + 1;
				}
				else
				{
					dictionary.Add(num4, 1);
				}
			}
			Log.Message(string.Concat(new object[]
			{
				"Values between ",
				num,
				" and ",
				num2,
				" (value: number of occurrences):"
			}), false);
			for (int j = num; j <= num2; j++)
			{
				Log.Message(j + ": " + dictionary[j], false);
			}
			return;
			Block_3:
			Log.Error("Failed to find all numbers in a range.", false);
		}

		private static void CheckIntsDistribution()
		{
			List<int> list = new List<int>();
			for (int j = 0; j < 1000000; j++)
			{
				int num = Rand.RangeInclusive(-2, 1);
				list.Add(num + 2);
			}
			Log.Message("Ints distribution (should be even):", false);
			int i;
			for (i = 0; i < 4; i++)
			{
				Log.Message(string.Concat(new object[]
				{
					i,
					": ",
					(float)list.Count((int x) => x == i) / (float)list.Count<int>() * 100f,
					"%"
				}), false);
			}
		}

		private static void CheckSeed()
		{
			int seed = 10;
			Rand.Seed = seed;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.Seed = seed;
			int int3 = Rand.Int;
			int int4 = Rand.Int;
			if (@int != int3 || int2 != int4)
			{
				Log.Error("Same seed, different values.", false);
			}
			Autotests_RandomNumbers.TestPushSeed(15, 20);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 20);
			Autotests_RandomNumbers.TestPushSeed(6, int.MaxValue);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 2147483642);
			Autotests_RandomNumbers.TestPushSeed(-1947483645, 1147483642);
			Autotests_RandomNumbers.TestPushSeed(455, 648023);
		}

		private static void TestPushSeed(int seed1, int seed2)
		{
			Rand.Seed = seed1;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.PushState();
			Rand.Seed = seed2;
			int int3 = Rand.Int;
			Rand.PopState();
			Rand.Seed = seed1;
			int int4 = Rand.Int;
			Rand.PushState();
			Rand.Seed = seed2;
			int int5 = Rand.Int;
			Rand.PopState();
			int int6 = Rand.Int;
			if (@int != int4 || int2 != int6 || int3 != int5)
			{
				Log.Error("PushSeed broken.", false);
			}
		}

		[CompilerGenerated]
		private static bool <CheckSimpleFloats>m__0(float x)
		{
			return x < 0f || x > 1f;
		}

		[CompilerGenerated]
		private static bool <CheckSimpleFloats>m__1(float x)
		{
			return x < 0.1f;
		}

		[CompilerGenerated]
		private static bool <CheckSimpleFloats>m__2(float x)
		{
			return (double)x > 0.5 && (double)x < 0.6;
		}

		[CompilerGenerated]
		private static bool <CheckSimpleFloats>m__3(float x)
		{
			return (double)x > 0.9;
		}

		[CompilerGenerated]
		private static bool <CheckSimpleFloats>m__4(float x)
		{
			return (double)x < 0.1;
		}

		[CompilerGenerated]
		private static bool <CheckSimpleFloats>m__5(float x)
		{
			return (double)x < 0.0001;
		}

		[CompilerGenerated]
		private sealed class <RandomFloats>c__Iterator0 : IEnumerable, IEnumerable<float>, IEnumerator, IDisposable, IEnumerator<float>
		{
			internal int <i>__1;

			internal int count;

			internal float $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RandomFloats>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < count)
				{
					this.$current = Rand.Value;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			float IEnumerator<float>.Current
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
				return this.System.Collections.Generic.IEnumerable<float>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<float> IEnumerable<float>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Autotests_RandomNumbers.<RandomFloats>c__Iterator0 <RandomFloats>c__Iterator = new Autotests_RandomNumbers.<RandomFloats>c__Iterator0();
				<RandomFloats>c__Iterator.count = count;
				return <RandomFloats>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CheckIntsDistribution>c__AnonStorey1
		{
			internal int i;

			public <CheckIntsDistribution>c__AnonStorey1()
			{
			}

			internal bool <>m__0(int x)
			{
				return x == this.i;
			}
		}
	}
}
