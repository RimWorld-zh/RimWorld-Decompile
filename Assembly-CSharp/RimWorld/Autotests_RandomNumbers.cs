using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class Autotests_RandomNumbers
	{
		public static void Run()
		{
			Log.Message("Running random numbers tests.");
			Autotests_RandomNumbers.CheckSimpleFloats();
			Autotests_RandomNumbers.CheckIntsRange();
			Autotests_RandomNumbers.CheckIntsDistribution();
			Autotests_RandomNumbers.CheckSeed();
			Log.Message("Finished.");
		}

		private static void CheckSimpleFloats()
		{
			List<float> list = Autotests_RandomNumbers.RandomFloats(500).ToList();
			if (list.Any((float x) => x < 0.0 || x > 1.0))
			{
				Log.Error("Float out of range.");
			}
			if (!list.Any((float x) => x < 0.10000000149011612) || !list.Any((float x) => (double)x > 0.5 && (double)x < 0.6) || !list.Any((float x) => (double)x > 0.9))
			{
				Log.Warning("Possibly uneven distribution.");
			}
			list = Autotests_RandomNumbers.RandomFloats(1300000).ToList();
			int num = list.Count((float x) => (double)x < 0.1);
			Log.Message("< 0.1 count (should be ~10%): " + (float)((float)num / (float)list.Count() * 100.0) + "%");
			num = list.Count((float x) => (double)x < 0.0001);
			Log.Message("< 0.0001 count (should be ~0.01%): " + (float)((float)num / (float)list.Count() * 100.0) + "%");
		}

		private static IEnumerable<float> RandomFloats(int count)
		{
			int i = 0;
			if (i < count)
			{
				yield return Rand.Value;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		private static void CheckIntsRange()
		{
			int num = -7;
			int num2 = 4;
			int num3 = 0;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			while (true)
			{
				bool flag = true;
				int num4 = num;
				while (num4 <= num2)
				{
					if (dictionary.ContainsKey(num4))
					{
						num4++;
						continue;
					}
					flag = false;
					break;
				}
				if (!flag)
				{
					num3++;
					if (num3 == 200000)
					{
						Log.Error("Failed to find all numbers in a range.");
						return;
					}
					int num5 = Rand.RangeInclusive(num, num2);
					if (num5 < num || num5 > num2)
					{
						Log.Error("Value out of range.");
					}
					if (dictionary.ContainsKey(num5))
					{
						Dictionary<int, int> dictionary2;
						int key;
						(dictionary2 = dictionary)[key = num5] = dictionary2[key] + 1;
					}
					else
					{
						dictionary.Add(num5, 1);
					}
					continue;
				}
				break;
			}
			Log.Message("Values between " + num + " and " + num2 + " (value: number of occurrences):");
			for (int i = num; i <= num2; i++)
			{
				Log.Message(i + ": " + dictionary[i]);
			}
		}

		private static void CheckIntsDistribution()
		{
			List<int> list = new List<int>();
			for (int j = 0; j < 1000000; j++)
			{
				int num = Rand.RangeInclusive(-2, 1);
				list.Add(num + 2);
			}
			Log.Message("Ints distribution (should be even):");
			int i;
			for (i = 0; i < 4; i++)
			{
				Log.Message(i + ": " + (float)((float)list.Count((int x) => x == i) / (float)list.Count() * 100.0) + "%");
			}
		}

		private static void CheckSeed()
		{
			int seed = Rand.Seed = 10;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.Seed = seed;
			int int3 = Rand.Int;
			int int4 = Rand.Int;
			if (@int != int3 || int2 != int4)
			{
				Log.Error("Same seed, different values.");
			}
			Autotests_RandomNumbers.TestPushSeed(15, 20);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 20);
			Autotests_RandomNumbers.TestPushSeed(6, 2147483647);
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
			if (@int == int4 && int2 == int6 && int3 == int5)
				return;
			Log.Error("PushSeed broken.");
		}
	}
}
