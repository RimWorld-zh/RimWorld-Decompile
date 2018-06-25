using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A9F RID: 2719
	public static class TouchPathEndModeUtility
	{
		// Token: 0x06003C88 RID: 15496 RVA: 0x002003EC File Offset: 0x001FE7EC
		public static bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z, Map map)
		{
			Building building = map.edificeGrid[new IntVec3(cornerX, 0, cornerZ)];
			bool result;
			if (building != null && TouchPathEndModeUtility.MakesOccupiedCellsAlwaysReachableDiagonally(building.def))
			{
				result = true;
			}
			else
			{
				IntVec3 intVec = new IntVec3(adjCardinal1X, 0, adjCardinal1Z);
				IntVec3 intVec2 = new IntVec3(adjCardinal2X, 0, adjCardinal2Z);
				result = ((map.pathGrid.Walkable(intVec) && intVec.GetDoor(map) == null) || (map.pathGrid.Walkable(intVec2) && intVec2.GetDoor(map) == null));
			}
			return result;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x00200490 File Offset: 0x001FE890
		public static bool MakesOccupiedCellsAlwaysReachableDiagonally(ThingDef def)
		{
			ThingDef thingDef = (!def.IsFrame) ? def : (def.entityDefToBuild as ThingDef);
			return thingDef != null && thingDef.CanInteractThroughCorners;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x002004DC File Offset: 0x001FE8DC
		public static bool IsAdjacentCornerAndNotAllowed(IntVec3 cell, IntVec3 BL, IntVec3 TL, IntVec3 TR, IntVec3 BR, Map map)
		{
			return (cell == BL && !TouchPathEndModeUtility.IsCornerTouchAllowed(BL.x + 1, BL.z + 1, BL.x + 1, BL.z, BL.x, BL.z + 1, map)) || (cell == TL && !TouchPathEndModeUtility.IsCornerTouchAllowed(TL.x + 1, TL.z - 1, TL.x + 1, TL.z, TL.x, TL.z - 1, map)) || (cell == TR && !TouchPathEndModeUtility.IsCornerTouchAllowed(TR.x - 1, TR.z - 1, TR.x - 1, TR.z, TR.x, TR.z - 1, map)) || (cell == BR && !TouchPathEndModeUtility.IsCornerTouchAllowed(BR.x - 1, BR.z + 1, BR.x - 1, BR.z, BR.x, BR.z + 1, map));
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x00200638 File Offset: 0x001FEA38
		public static void AddAllowedAdjacentRegions(LocalTargetInfo dest, TraverseParms traverseParams, Map map, List<Region> regions)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(dest, out bl, out tl, out tr, out br);
			if (!dest.HasThing || (dest.Thing.def.size.x == 1 && dest.Thing.def.size.z == 1))
			{
				IntVec3 cell = dest.Cell;
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = GenAdj.AdjacentCells[i] + cell;
					if (intVec.InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(intVec, bl, tl, tr, br, map))
					{
						Region region = intVec.GetRegion(map, RegionType.Set_Passable);
						if (region != null && region.Allows(traverseParams, true))
						{
							regions.Add(region);
						}
					}
				}
			}
			else
			{
				List<IntVec3> list = GenAdjFast.AdjacentCells8Way(dest);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].InBounds(map) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(list[j], bl, tl, tr, br, map))
					{
						Region region2 = list[j].GetRegion(map, RegionType.Set_Passable);
						if (region2 != null && region2.Allows(traverseParams, true))
						{
							regions.Add(region2);
						}
					}
				}
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x002007B0 File Offset: 0x001FEBB0
		public static bool IsAdjacentOrInsideAndAllowedToTouch(IntVec3 root, LocalTargetInfo target, Map map)
		{
			IntVec3 bl;
			IntVec3 tl;
			IntVec3 tr;
			IntVec3 br;
			GenAdj.GetAdjacentCorners(target, out bl, out tl, out tr, out br);
			return root.AdjacentTo8WayOrInside(target) && !TouchPathEndModeUtility.IsAdjacentCornerAndNotAllowed(root, bl, tl, tr, br, map);
		}
	}
}
