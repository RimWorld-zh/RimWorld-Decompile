using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class SiteGenStepUtility
	{
		public static bool TryFindRootToSpawnAroundRectOfInterest(out CellRect rectToDefend, out IntVec3 singleCellToSpawnNear, Map map)
		{
			singleCellToSpawnNear = IntVec3.Invalid;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.Empty;
				if (!RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= 225, map, out singleCellToSpawnNear))
				{
					return false;
				}
			}
			return true;
		}

		public static bool TryFindSpawnCellAroundOrNear(CellRect around, IntVec3 near, Map map, out IntVec3 spawnCell)
		{
			if (near.IsValid)
			{
				if (!CellFinder.TryFindRandomSpawnCellForPawnNear(near, map, out spawnCell, 10))
				{
					return false;
				}
			}
			else if (!CellFinder.TryFindRandomCellInsideWith(around.ExpandedBy(8), (IntVec3 x) => !around.Contains(x) && x.InBounds(map) && x.Standable(map) && !x.Fogged(map), out spawnCell))
			{
				return false;
			}
			return true;
		}

		[CompilerGenerated]
		private sealed class <TryFindRootToSpawnAroundRectOfInterest>c__AnonStorey0
		{
			internal Map map;

			public <TryFindRootToSpawnAroundRectOfInterest>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && !x.Fogged(this.map) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= 225;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindSpawnCellAroundOrNear>c__AnonStorey1
		{
			internal CellRect around;

			internal Map map;

			public <TryFindSpawnCellAroundOrNear>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return !this.around.Contains(x) && x.InBounds(this.map) && x.Standable(this.map) && !x.Fogged(this.map);
			}
		}
	}
}
