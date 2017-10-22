using RimWorld;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class GenGrid
	{
		public const int NoBuildEdgeWidth = 10;

		public const int NoZoneEdgeWidth = 5;

		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			bool result;
			if (dir == Rot4.North)
			{
				result = (c.z == 0);
			}
			else if (dir == Rot4.South)
			{
				int z = c.z;
				IntVec3 size = map.Size;
				result = (z == size.z - 1);
			}
			else if (dir == Rot4.West)
			{
				result = (c.x == 0);
			}
			else if (dir == Rot4.East)
			{
				int x = c.x;
				IntVec3 size2 = map.Size;
				result = (x == size2.x - 1);
			}
			else
			{
				Log.ErrorOnce("Invalid edge direction", 55370769);
				result = false;
			}
			return result;
		}

		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (uint)c.x < size.x && (uint)c.z < size.z;
		}

		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0.0 && v.z >= 0.0 && v.x < (float)size.x && v.z < (float)size.z;
		}

		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathGrid.Walkable(c);
		}

		public static bool Standable(this IntVec3 c, Map map)
		{
			bool result;
			if (!map.pathGrid.Walkable(c))
			{
				result = false;
			}
			else
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.passability != 0)
						goto IL_0044;
				}
				result = true;
			}
			goto IL_0063;
			IL_0044:
			result = false;
			goto IL_0063;
			IL_0063:
			return result;
		}

		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < list.Count)
				{
					if (list[num].def.passability == Traversability.Impassable)
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordance surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		public static bool CanBeSeenOver(this IntVec3 c, Map map)
		{
			bool result;
			if (!c.InBounds(map))
			{
				result = false;
			}
			else
			{
				Building edifice = c.GetEdifice(map);
				result = ((byte)((edifice == null || edifice.CanBeSeenOver()) ? 1 : 0) != 0);
			}
			return result;
		}

		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return (byte)((edifice == null || edifice.CanBeSeenOver()) ? 1 : 0) != 0;
		}

		public static bool CanBeSeenOver(this Building b)
		{
			bool result;
			if (b.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = b as Building_Door;
				result = ((byte)((building_Door != null && building_Door.Open) ? 1 : 0) != 0);
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			SurfaceType result;
			List<Thing> thingList;
			int i;
			if (!c.InBounds(map))
			{
				result = SurfaceType.None;
			}
			else
			{
				thingList = c.GetThingList(map);
				for (i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.surfaceType != 0)
						goto IL_003a;
				}
				result = SurfaceType.None;
			}
			goto IL_0069;
			IL_003a:
			result = thingList[i].def.surfaceType;
			goto IL_0069;
			IL_0069:
			return result;
		}

		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}
	}
}
