using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005AE RID: 1454
	public static class GenWorldClosest
	{
		// Token: 0x06001BD1 RID: 7121 RVA: 0x000EFD34 File Offset: 0x000EE134
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

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000EFDA0 File Offset: 0x000EE1A0
		public static bool TryFindClosestPassableTile(int rootTile, out int foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (int x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
