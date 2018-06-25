using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005AE RID: 1454
	public static class GenWorldClosest
	{
		// Token: 0x06001BD2 RID: 7122 RVA: 0x000EFACC File Offset: 0x000EDECC
		public static bool TryFindClosestTile(int rootTile, Predicate<int> predicate, out int foundTile, int maxTilesToScan = 2147483647, bool canSearchThroughImpassable = true)
		{
			int foundTileLocal = -1;
			Find.WorldFloodFiller.FloodFill(rootTile, (int x) => canSearchThroughImpassable || !Find.World.Impassable(x), delegate(int t)
			{
				bool flag = predicate(t);
				if (flag)
				{
					foundTileLocal = t;
				}
				return flag;
			}, maxTilesToScan, null);
			foundTile = foundTileLocal;
			return foundTileLocal >= 0;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000EFB38 File Offset: 0x000EDF38
		public static bool TryFindClosestPassableTile(int rootTile, out int foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (int x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
