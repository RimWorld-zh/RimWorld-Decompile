using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097C RID: 2428
	public static class FuelingPortUtility
	{
		// Token: 0x06003697 RID: 13975 RVA: 0x001D1A58 File Offset: 0x001CFE58
		public static IntVec3 GetFuelingPortCell(Building podLauncher)
		{
			return FuelingPortUtility.GetFuelingPortCell(podLauncher.Position, podLauncher.Rotation);
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x001D1A80 File Offset: 0x001CFE80
		public static IntVec3 GetFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			rot.Rotate(RotationDirection.Clockwise);
			return center + rot.FacingCell;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x001D1AAC File Offset: 0x001CFEAC
		public static bool AnyFuelingPortGiverAt(IntVec3 c, Map map)
		{
			return FuelingPortUtility.FuelingPortGiverAt(c, map) != null;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x001D1AD0 File Offset: 0x001CFED0
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

		// Token: 0x0600369B RID: 13979 RVA: 0x001D1B38 File Offset: 0x001CFF38
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

		// Token: 0x0600369C RID: 13980 RVA: 0x001D1BF4 File Offset: 0x001CFFF4
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
