using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F7 RID: 2295
	public static class Autotests_RandomNumbers
	{
		// Token: 0x0600351E RID: 13598 RVA: 0x001C6B2A File Offset: 0x001C4F2A
		public static void Run()
		{
			Log.Message("Running random numbers tests.", false);
			Autotests_RandomNumbers.CheckSimpleFloats();
			Autotests_RandomNumbers.CheckIntsRange();
			Autotests_RandomNumbers.CheckIntsDistribution();
			Autotests_RandomNumbers.CheckSeed();
			Log.Message("Finished.", false);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x001C6B58 File Offset: 0x001C4F58
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

		// Token: 0x06003520 RID: 13600 RVA: 0x001C6CD8 File Offset: 0x001C50D8
		private static IEnumerable<float> RandomFloats(int count)
		{
			for (int i = 0; i < count; i++)
			{
				yield return Rand.Value;
			}
			yield break;
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x001C6D04 File Offset: 0x001C5104
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

		// Token: 0x06003522 RID: 13602 RVA: 0x001C6E54 File Offset: 0x001C5254
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

		// Token: 0x06003523 RID: 13603 RVA: 0x001C6F24 File Offset: 0x001C5324
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

		// Token: 0x06003524 RID: 13604 RVA: 0x001C6FC4 File Offset: 0x001C53C4
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
	}
}
