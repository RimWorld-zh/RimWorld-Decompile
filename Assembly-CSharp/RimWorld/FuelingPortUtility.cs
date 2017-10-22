using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class FuelingPortUtility
	{
		public static IntVec3 GetFuelingPortCell(Building podLauncher)
		{
			return FuelingPortUtility.GetFuelingPortCell(podLauncher.Position, podLauncher.Rotation);
		}

		public static IntVec3 GetFuelingPortCell(IntVec3 center, Rot4 rot)
		{
			rot.Rotate(RotationDirection.Clockwise);
			return center + rot.FacingCell;
		}

		public static bool AnyFuelingPortGiverAt(IntVec3 c, Map map)
		{
			return FuelingPortUtility.FuelingPortGiverAt(c, map) != null;
		}

		public static Building FuelingPortGiverAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Building result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Building building = thingList[num] as Building;
					if (building != null && building.def.building.hasFuelingPort)
					{
						result = building;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static Building FuelingPortGiverAtFuelingPortCell(IntVec3 c, Map map)
		{
			int num = 0;
			Building result;
			while (true)
			{
				Building building;
				if (num < 4)
				{
					IntVec3 c2 = c + GenAdj.CardinalDirections[num];
					if (c2.InBounds(map))
					{
						List<Thing> thingList = c2.GetThingList(map);
						for (int i = 0; i < thingList.Count; i++)
						{
							building = (thingList[i] as Building);
							if (building != null && building.def.building.hasFuelingPort && FuelingPortUtility.GetFuelingPortCell(building) == c)
								goto IL_007e;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
				IL_007e:
				result = building;
				break;
			}
			return result;
		}

		public static CompLaunchable LaunchableAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			CompLaunchable result;
			while (true)
			{
				if (num < thingList.Count)
				{
					CompLaunchable compLaunchable = thingList[num].TryGetComp<CompLaunchable>();
					if (compLaunchable != null)
					{
						result = compLaunchable;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
