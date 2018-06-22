using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097A RID: 2426
	public static class FuelingPortUtility
	{
		// Token: 0x06003693 RID: 13971 RVA: 0x001D1918 File Offset: 0x001CFD18
		public static IntVec3 GetFuelingPortCell(Building podLauncher)
		{
			return FuelingPortUtility.GetFuelingPortCell(podLauncher.Position, podLauncher.Rotation);
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x001D1940 File Offset: 0x001CFD40
		public static IntVec3 GetFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			rot.Rotate(RotationDirection.Clockwise);
			return center + rot.FacingCell;
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x001D196C File Offset: 0x001CFD6C
		public static bool AnyFuelingPortGiverAt(IntVec3 c, Map map)
		{
			return FuelingPortUtility.FuelingPortGiverAt(c, map) != null;
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x001D1990 File Offset: 0x001CFD90
		public static Building FuelingPortGiverAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Building building = thingList[i] as Building;
				if (building != null && building.def.building.hasFuelingPort)
				{
					return building;
				}
			}
			return null;
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x001D19F8 File Offset: 0x001CFDF8
		public static Building FuelingPortGiverAtFuelingPortCell(IntVec3 c, Map map)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c2 = c + GenAdj.CardinalDirections[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Building building = thingList[j] as Building;
						if (building != null && building.def.building.hasFuelingPort && FuelingPortUtility.GetFuelingPortCell(building) == c)
						{
							return building;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x001D1AB4 File Offset: 0x001CFEB4
		public static CompLaunchable LaunchableAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CompLaunchable compLaunchable = thingList[i].TryGetComp<CompLaunchable>();
				if (compLaunchable != null)
				{
					return compLaunchable;
				}
			}
			return null;
		}
	}
}
