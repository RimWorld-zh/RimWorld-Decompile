using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F44 RID: 3908
	public static class GenGrid
	{
		// Token: 0x04003E17 RID: 15895
		public const int NoBuildEdgeWidth = 10;

		// Token: 0x04003E18 RID: 15896
		public const int NoZoneEdgeWidth = 5;

		// Token: 0x06005E55 RID: 24149 RVA: 0x002FFB94 File Offset: 0x002FDF94
		public static bool InNoBuildEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 10);
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x002FFBB4 File Offset: 0x002FDFB4
		public static bool InNoZoneEdgeArea(this IntVec3 c, Map map)
		{
			return c.CloseToEdge(map, 5);
		}

		// Token: 0x06005E57 RID: 24151 RVA: 0x002FFBD4 File Offset: 0x002FDFD4
		public static bool CloseToEdge(this IntVec3 c, Map map, int edgeDist)
		{
			IntVec3 size = map.Size;
			return c.x < edgeDist || c.z < edgeDist || c.x >= size.x - edgeDist || c.z >= size.z - edgeDist;
		}

		// Token: 0x06005E58 RID: 24152 RVA: 0x002FFC38 File Offset: 0x002FE038
		public static bool OnEdge(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return c.x == 0 || c.x == size.x - 1 || c.z == 0 || c.z == size.z - 1;
		}

		// Token: 0x06005E59 RID: 24153 RVA: 0x002FFC98 File Offset: 0x002FE098
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

		// Token: 0x06005E5A RID: 24154 RVA: 0x002FFD60 File Offset: 0x002FE160
		public static bool InBounds(this IntVec3 c, Map map)
		{
			IntVec3 size = map.Size;
			return (ulong)c.x < (ulong)((long)size.x) && (ulong)c.z < (ulong)((long)size.z);
		}

		// Token: 0x06005E5B RID: 24155 RVA: 0x002FFDA8 File Offset: 0x002FE1A8
		public static bool InBounds(this Vector3 v, Map map)
		{
			IntVec3 size = map.Size;
			return v.x >= 0f && v.z >= 0f && v.x < (float)size.x && v.z < (float)size.z;
		}

		// Token: 0x06005E5C RID: 24156 RVA: 0x002FFE10 File Offset: 0x002FE210
		public static bool Walkable(this IntVec3 c, Map map)
		{
			return map.pathGrid.Walkable(c);
		}

		// Token: 0x06005E5D RID: 24157 RVA: 0x002FFE34 File Offset: 0x002FE234
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

		// Token: 0x06005E5E RID: 24158 RVA: 0x002FFEA8 File Offset: 0x002FE2A8
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

		// Token: 0x06005E5F RID: 24159 RVA: 0x002FFF04 File Offset: 0x002FE304
		public static bool SupportsStructureType(this IntVec3 c, Map map, TerrainAffordanceDef surfaceType)
		{
			return c.GetTerrain(map).affordances.Contains(surfaceType);
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x002FFF2C File Offset: 0x002FE32C
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

		// Token: 0x06005E61 RID: 24161 RVA: 0x002FFF78 File Offset: 0x002FE378
		public static bool CanBeSeenOverFast(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice == null || edifice.CanBeSeenOver();
		}

		// Token: 0x06005E62 RID: 24162 RVA: 0x002FFFB0 File Offset: 0x002FE3B0
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

		// Token: 0x06005E63 RID: 24163 RVA: 0x00300000 File Offset: 0x002FE400
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

		// Token: 0x06005E64 RID: 24164 RVA: 0x00300078 File Offset: 0x002FE478
		public static bool HasEatSurface(this IntVec3 c, Map map)
		{
			return c.GetSurfaceType(map) == SurfaceType.Eat;
		}
	}
}
