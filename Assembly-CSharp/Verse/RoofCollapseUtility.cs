using System;

namespace Verse
{
	// Token: 0x02000CA1 RID: 3233
	public static class RoofCollapseUtility
	{
		// Token: 0x06004721 RID: 18209 RVA: 0x00257918 File Offset: 0x00255D18
		public static bool WithinRangeOfRoofHolder(IntVec3 c, Map map, bool assumeNonNoRoofCellsAreRoofed = false)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => (x.Roofed(map) || x == c || (assumeNonNoRoofCellsAreRoofed && !map.areaManager.NoRoof[x])) && x.InHorDistOf(c, 6.9f), delegate(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map) && c2.InHorDistOf(c, 6.9f))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							return true;
						}
					}
				}
				return false;
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0025798C File Offset: 0x00255D8C
		public static bool ConnectedToRoofHolder(IntVec3 c, Map map, bool assumeRoofAtRoot)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => (x.Roofed(map) || (x == c && assumeRoofAtRoot)) && !connected, delegate(IntVec3 x)
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
					if (c2.InBounds(map))
					{
						Building edifice = c2.GetEdifice(map);
						if (edifice != null && edifice.def.holdsRoof)
						{
							connected = true;
							break;
						}
					}
				}
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x04003055 RID: 12373
		public const float RoofMaxSupportDistance = 6.9f;

		// Token: 0x04003056 RID: 12374
		public static readonly int RoofSupportRadialCellsCount = GenRadial.NumCellsInRadius(6.9f);
	}
}
