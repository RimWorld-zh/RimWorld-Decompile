using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C90 RID: 3216
	public static class RegionListersUpdater
	{
		// Token: 0x04003010 RID: 12304
		private static List<Region> tmpRegions = new List<Region>();

		// Token: 0x06004697 RID: 18071 RVA: 0x002540B8 File Offset: 0x002524B8
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

		// Token: 0x06004698 RID: 18072 RVA: 0x00254138 File Offset: 0x00252538
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

		// Token: 0x06004699 RID: 18073 RVA: 0x002541AC File Offset: 0x002525AC
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

		// Token: 0x0600469A RID: 18074 RVA: 0x00254204 File Offset: 0x00252604
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

		// Token: 0x0600469B RID: 18075 RVA: 0x002542E8 File Offset: 0x002526E8
		private static bool CanRegisterInAdjacentRegions(Thing thing)
		{
			return true;
		}
	}
}
