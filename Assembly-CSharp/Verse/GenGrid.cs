using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F40 RID: 3904
	public static class GenGrid
	{
		// Token: 0x06005E23 RID: 24099 RVA: 0x002FD4D8 File Offset: 0x002FB8D8
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		// Token: 0x06005E24 RID: 24100 RVA: 0x002FD4F8 File Offset: 0x002FB8F8
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x06005E25 RID: 24101 RVA: 0x002FD518 File Offset: 0x002FB918
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x002FD57C File Offset: 0x002FB97C
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x002FD5DC File Offset: 0x002FB9DC
		public static bool OnEdge(this IntVec3 c, Map map, Rot4 dir)
		{
			bool result;
			if (dir == Rot4.North)
			{
				result = (c.z == 0);
			}
			else if (dir == Rot4.South)
			{
				result = (c.z == map.Size.z - 1);
			}
			else if (dir == Rot4.West)
			{
				result = (c.x == 0);
			}
			else if (dir == Rot4.East)
			{
				result = (c.x == map.Size.x - 1);
			}
			else
			{
				Log.ErrorOnce("Invalid edge direction", 55370769, false);
				result = false;
			}
			return result;
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x002FD6A4 File Offset: 0x002FBAA4
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x002FD6EC File Offset: 0x002FBAEC
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x002FD754 File Offset: 0x002FBB54
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathGrid.Walkable(c);
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x002FD778 File Offset: 0x002FBB78
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
					if (list[i].def.passability != Traversability.Standable)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005E2C RID: 24108 RVA: 0x002FD7EC File Offset: 0x002FBBEC
		public static bool Impassable(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.passability == Traversability.Impassable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005E2D RID: 24109 RVA: 0x002FD848 File Offset: 0x002FBC48
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x002FD870 File Offset: 0x002FBC70
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
				result = (edifice == null || edifice.CanBeSeenOver());
			}
			return result;
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x002FD8BC File Offset: 0x002FBCBC
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x002FD8F4 File Offset: 0x002FBCF4
		public static bool CanBeSeenOver(this Building b)
		{
			bool result;
			if (b.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = b as Building_Door;
				result = (building_Door != null && building_Door.Open);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06005E31 RID: 24113 RVA: 0x002FD944 File Offset: 0x002FBD44
		public static SurfaceType GetSurfaceType(this IntVec3 c, Map map)
		{
			SurfaceType result;
			if (!c.InBounds(map))
			{
				result = SurfaceType.None;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.surfaceType != SurfaceType.None)
					{
						return thingList[i].def.surfaceType;
					}
				}
				result = SurfaceType.None;
			}
			return result;
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x002FD9BC File Offset: 0x002FBDBC
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x04003E02 RID: 15874
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x04003E03 RID: 15875
		public const int NoZoneEdgeWidth = 5;
	}
}
