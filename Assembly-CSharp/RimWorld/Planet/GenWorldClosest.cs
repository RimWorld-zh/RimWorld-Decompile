using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public static class GenWorldClosest
	{
		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache0;

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

		public static bool TryFindClosestPassableTile(int rootTile, out int foundTile)
		{
			return GenWorldClosest.TryFindClosestTile(rootTile, (int x) => !Find.World.Impassable(x), out foundTile, int.MaxValue, true);
		}

		[CompilerGenerated]
		private static bool <TryFindClosestPassableTile>m__0(int x)
		{
			return !Find.World.Impassable(x);
		}

		[CompilerGenerated]
		private sealed class <TryFindClosestTile>c__AnonStorey0
		{
			internal bool canSearchThroughImpassable;

			internal Predicate<int> predicate;

			internal int foundTileLocal;

			public <TryFindClosestTile>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int x)
			{
				return this.canSearchThroughImpassable || !Find.World.Impassable(x);
			}

			internal bool <>m__1(int t)
			{
				bool flag = this.predicate(t);
				if (flag)
				{
					this.foundTileLocal = t;
				}
				return flag;
			}
		}
	}
}
