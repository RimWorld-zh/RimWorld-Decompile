using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097E RID: 2430
	public static class FuelingPortUtility
	{
		// Token: 0x0600369A RID: 13978 RVA: 0x001D1730 File Offset: 0x001CFB30
		public static IntVec3 GetFuelingPortCell(Building podLauncher)
		{
			return FuelingPortUtility.GetFuelingPortCell(podLauncher.Position, podLauncher.Rotation);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x001D1758 File Offset: 0x001CFB58
		public static IntVec3 GetFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			rot.Rotate(RotationDirection.Clockwise);
			return center + rot.FacingCell;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x001D1784 File Offset: 0x001CFB84
		public static bool AnyFuelingPortGiverAt(IntVec3 c, Map map)
		{
			return FuelingPortUtility.FuelingPortGiverAt(c, map) != null;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001D17A8 File Offset: 0x001CFBA8
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

		// Token: 0x0600369E RID: 13982 RVA: 0x001D1810 File Offset: 0x001CFC10
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

		// Token: 0x0600369F RID: 13983 RVA: 0x001D18CC File Offset: 0x001CFCCC
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
