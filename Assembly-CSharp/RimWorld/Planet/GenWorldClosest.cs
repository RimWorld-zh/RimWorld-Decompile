using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005AC RID: 1452
	public static class GenWorldClosest
	{
		// Token: 0x06001BCE RID: 7118 RVA: 0x000EF97C File Offset: 0x000EDD7C
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

		// Token: 0x06001BCF RID: 7119 RVA: 0x000EF9E8 File Offset: 0x000EDDE8
		public static bool TryFindClosestPassableTile(int rootTile, out int foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (int x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}
	}
}
