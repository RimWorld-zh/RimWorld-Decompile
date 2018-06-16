using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C92 RID: 3218
	public static class RegionListersUpdater
	{
		// Token: 0x0600468D RID: 18061 RVA: 0x00252C34 File Offset: 0x00251034
		public static void DeregisterInRegions(Thing thing, Map map)
		{
			ThingDef def = thing.def;
			if (ListerThings.EverListable(def, ListerThingsUse.Region))
			{
				RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, true);
				for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
				{
					ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
					if (listerThings.Contains(thing))
					{
						listerThings.Remove(thing);
					}
				}
				RegionListersUpdater.tmpRegions.Clear();
			}
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x00252CB4 File Offset: 0x002510B4
		public static void RegisterInRegions(Thing thing, Map map)
		{
			ThingDef def = thing.def;
			if (ListerThings.EverListable(def, ListerThingsUse.Region))
			{
				RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, false);
				for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
				{
					ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
					if (!listerThings.Contains(thing))
					{
						listerThings.Add(thing);
					}
				}
			}
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x00252D28 File Offset: 0x00251128
		public static void RegisterAllAt(IntVec3 c, Map map, HashSet<Thing> processedThings = null)
		{
			List<Thing> thingList = c.GetThingList(map);
			int count = thingList.Count;
			for (int i = 0; i < count; i++)
			{
				Thing thing = thingList[i];
				if (processedThings == null || processedThings.Add(thing))
				{
					RegionListersUpdater.RegisterInRegions(thing, map);
				}
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00252D80 File Offset: 0x00251180
		public static void GetTouchableRegions(Thing thing, Map map, List<Region> outRegions, bool allowAdjacentEvenIfCantTouch = false)
		{
			outRegions.Clear();
			CellRect cellRect = thing.OccupiedRect();
			CellRect cellRect2 = cellRect;
			if (RegionListersUpdater.CanRegisterInAdjacentRegions(thing))
			{
				cellRect2 = cellRect2.ExpandedBy(1);
			}
			CellRect.CellRectIterator iterator = cellRect2.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				if (intVec.InBounds(map))
				{
					Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(intVec);
					if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.type.Passable() && !outRegions.Contains(validRegionAt_NoRebuild))
					{
						if (cellRect.Contains(intVec))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
						else if (allowAdjacentEvenIfCantTouch || ReachabilityImmediate.CanReachImmediate(intVec, thing, map, PathEndMode.Touch, null))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
					}
				}
				iterator.MoveNext();
			}
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x00252E64 File Offset: 0x00251264
		private static bool CanRegisterInAdjacentRegions(Thing thing)
		{
			return true;
		}

		// Token: 0x04003008 RID: 12296
		private static List<Region> tmpRegions = new List<Region>();
	}
}
