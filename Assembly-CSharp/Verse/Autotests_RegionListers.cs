using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020008F8 RID: 2296
	public static class Autotests_RegionListers
	{
		// Token: 0x04001CCA RID: 7370
		private static Dictionary<Region, List<Thing>> expectedListers = new Dictionary<Region, List<Thing>>();

		// Token: 0x04001CCB RID: 7371
		private static List<Region> tmpTouchableRegions = new List<Region>();

		// Token: 0x0600352B RID: 13611 RVA: 0x001C6FB5 File Offset: 0x001C53B5
		public static void CheckBugs(Map map)
		{
			Autotests_RegionListers.CalculateExpectedListers(map);
			Autotests_RegionListers.CheckThingRegisteredTwice(map);
			Autotests_RegionListers.CheckThingNotRegisteredButShould();
			Autotests_RegionListers.CheckThingRegisteredButShouldnt(map);
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x001C6FD0 File Offset: 0x001C53D0
		private static void CheckThingRegisteredTwice(Map map)
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				Autotests_RegionListers.CheckDuplicates(keyValuePair.Value, keyValuePair.Key, true);
			}
			foreach (Region region in map.regionGrid.AllRegions)
			{
				Autotests_RegionListers.CheckDuplicates(region.ListerThings.AllThings, region, false);
			}
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x001C7098 File Offset: 0x001C5498
		private static void CheckDuplicates(List<Thing> lister, Region region, bool expected)
		{
			for (int i = 1; i < lister.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (lister[i] == lister[j])
					{
						if (expected)
						{
							Log.Error(string.Concat(new object[]
							{
								"Region error: thing ",
								lister[i],
								" is expected to be registered twice in ",
								region,
								"? This should never happen."
							}), false);
						}
						else
						{
							Log.Error(string.Concat(new object[]
							{
								"Region error: thing ",
								lister[i],
								" is registered twice in ",
								region
							}), false);
						}
					}
				}
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x001C715C File Offset: 0x001C555C
		private static void CheckThingNotRegisteredButShould()
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				List<Thing> value = keyValuePair.Value;
				List<Thing> allThings = keyValuePair.Key.ListerThings.AllThings;
				for (int i = 0; i < value.Count; i++)
				{
					if (!allThings.Contains(value[i]))
					{
						Log.Error(string.Concat(new object[]
						{
							"Region error: thing ",
							value[i],
							" at ",
							value[i].Position,
							" should be registered in ",
							keyValuePair.Key,
							" but it's not."
						}), false);
					}
				}
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x001C725C File Offset: 0x001C565C
		private static void CheckThingRegisteredButShouldnt(Map map)
		{
			foreach (Region region in map.regionGrid.AllRegions)
			{
				List<Thing> list;
				if (!Autotests_RegionListers.expectedListers.TryGetValue(region, out list))
				{
					list = null;
				}
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					if (list == null || !list.Contains(allThings[i]))
					{
						Log.Error(string.Concat(new object[]
						{
							"Region error: thing ",
							allThings[i],
							" at ",
							allThings[i].Position,
							" is registered in ",
							region,
							" but it shouldn't be."
						}), false);
					}
				}
			}
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x001C7364 File Offset: 0x001C5764
		private static void CalculateExpectedListers(Map map)
		{
			Autotests_RegionListers.expectedListers.Clear();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if (ListerThings.EverListable(thing.def, ListerThingsUse.Region))
				{
					RegionListersUpdater.GetTouchableRegions(thing, map, Autotests_RegionListers.tmpTouchableRegions, false);
					for (int j = 0; j < Autotests_RegionListers.tmpTouchableRegions.Count; j++)
					{
						Region key = Autotests_RegionListers.tmpTouchableRegions[j];
						List<Thing> list;
						if (!Autotests_RegionListers.expectedListers.TryGetValue(key, out list))
						{
							list = new List<Thing>();
							Autotests_RegionListers.expectedListers.Add(key, list);
						}
						list.Add(allThings[i]);
					}
				}
			}
		}
	}
}
