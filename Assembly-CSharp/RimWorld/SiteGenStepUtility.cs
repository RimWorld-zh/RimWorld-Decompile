using System;
using Verse;

namespace RimWorld
{
	public static class SiteGenStepUtility
	{
		public static bool TryFindRootToSpawnAroundRectOfInterest(out CellRect rectToDefend, out IntVec3 singleCellToSpawnNear, Map map)
		{
			singleCellToSpawnNear = IntVec3.Invalid;
			bool result;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.Empty;
				if (!RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((Predicate<IntVec3>)((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 225), map, out singleCellToSpawnNear))
				{
					result = false;
					goto IL_0062;
				}
			}
			result = true;
			goto IL_0062;
			IL_0062:
			return result;
		}

		public static bool TryFindSpawnCellAroundOrNear(CellRect around, IntVec3 near, Map map, out IntVec3 spawnCell)
		{
			bool result;
			if (near.IsValid)
			{
				if (!CellFinder.TryFindRandomSpawnCellForPawnNear(near, map, out spawnCell, 10))
				{
					result = false;
					goto IL_0076;
				}
			}
			else if (!CellFinder.TryFindRandomCellInsideWith(around.ExpandedBy(8), (Predicate<IntVec3>)((IntVec3 x) => !around.Contains(x) && x.InBounds(map) && x.Standable(map) && !x.Fogged(map)), out spawnCell))
			{
				result = false;
				goto IL_0076;
			}
			result = true;
			goto IL_0076;
			IL_0076:
			return result;
		}
	}
}
