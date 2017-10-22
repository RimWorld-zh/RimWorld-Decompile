using System;

namespace Verse
{
	public static class RoofCollapseUtility
	{
		public const float RoofMaxSupportDistance = 6.9f;

		public static readonly int RoofSupportRadialCellsCount = GenRadial.NumCellsInRadius(6.9f);

		public static bool WithinRangeOfRoofHolder(IntVec3 c, Map map)
		{
			CellIndices cellIndices = map.cellIndices;
			Building[] innerArray = map.edificeGrid.InnerArray;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < RoofCollapseUtility.RoofSupportRadialCellsCount)
				{
					IntVec3 c2 = c + GenRadial.RadialPattern[num];
					if (c2.InBounds(map))
					{
						Building building = innerArray[cellIndices.CellToIndex(c2)];
						if (building != null && building.def.holdsRoof)
						{
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public static bool ConnectedToRoofHolder(IntVec3 c, Map map, bool assumeRoofAtRoot)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (Predicate<IntVec3>)((IntVec3 x) => (x.Roofed(map) || (x == c && assumeRoofAtRoot)) && !connected), (Action<IntVec3>)delegate(IntVec3 x)
			{
				int num = 0;
				while (true)
				{
					if (num < 5)
					{
						IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[num];
						if (c2.InBounds(map))
						{
							Building edifice = c2.GetEdifice(map);
							if (edifice != null && edifice.def.holdsRoof)
								break;
						}
						num++;
						continue;
					}
					return;
				}
				connected = true;
			}, 2147483647, false, null);
			return connected;
		}
	}
}
