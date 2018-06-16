using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F41 RID: 3905
	public static class GenGrid
	{
		// Token: 0x06005E25 RID: 24101 RVA: 0x002FD3FC File Offset: 0x002FB7FC
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x002FD41C File Offset: 0x002FB81C
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x002FD43C File Offset: 0x002FB83C
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x002FD4A0 File Offset: 0x002FB8A0
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x002FD500 File Offset: 0x002FB900
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

		// Token: 0x06005E2A RID: 24106 RVA: 0x002FD5C8 File Offset: 0x002FB9C8
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x002FD610 File Offset: 0x002FBA10
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x06005E2C RID: 24108 RVA: 0x002FD678 File Offset: 0x002FBA78
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathGrid.Walkable(c);
		}

		// Token: 0x06005E2D RID: 24109 RVA: 0x002FD69C File Offset: 0x002FBA9C
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

		// Token: 0x06005E2E RID: 24110 RVA: 0x002FD710 File Offset: 0x002FBB10
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

		// Token: 0x06005E2F RID: 24111 RVA: 0x002FD76C File Offset: 0x002FBB6C
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x002FD794 File Offset: 0x002FBB94
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

		// Token: 0x06005E31 RID: 24113 RVA: 0x002FD7E0 File Offset: 0x002FBBE0
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x002FD818 File Offset: 0x002FBC18
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

		// Token: 0x06005E33 RID: 24115 RVA: 0x002FD868 File Offset: 0x002FBC68
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

		// Token: 0x06005E34 RID: 24116 RVA: 0x002FD8E0 File Offset: 0x002FBCE0
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}

		// Token: 0x04003E03 RID: 15875
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x04003E04 RID: 15876
		public const int NoZoneEdgeWidth = 5;
	}
}
