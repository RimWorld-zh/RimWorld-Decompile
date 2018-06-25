using System;

namespace Verse
{
	// Token: 0x02000CA0 RID: 3232
	public static class RoofCollapseUtility
	{
		// Token: 0x04003065 RID: 12389
		public const float RoofMaxSupportDistance = 6.9f;

		// Token: 0x04003066 RID: 12390
		public static readonly int RoofSupportRadialCellsCount = GenRadial.NumCellsInRadius(6.9f);

		// Token: 0x0600472B RID: 18219 RVA: 0x0025909C File Offset: 0x0025749C
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

		// Token: 0x0600472C RID: 18220 RVA: 0x00259110 File Offset: 0x00257510
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
	}
}
