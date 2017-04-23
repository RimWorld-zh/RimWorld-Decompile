using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	public static class TileFinder
	{
		private static List<Pair<int, int>> tmpTiles = new List<Pair<int, int>>();

		public static int RandomStartingTile()
		{
			return TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, true);
		}

		public static int RandomFactionBaseTileFor(Faction faction, bool mustBeAutoChoosable = false)
		{
			for (int i = 0; i < 500; i++)
			{
				int num;
				if ((from _ in Enumerable.Range(0, 100)
				select Rand.Range(0, Find.WorldGrid.TilesCount)).TryRandomElementByWeight(delegate(int x)
				{
					Tile tile = Find.WorldGrid[x];
					if (!tile.biome.canBuildBase || !tile.biome.implemented || tile.hilliness == Hilliness.Impassable)
					{
						return 0f;
					}
					if (mustBeAutoChoosable && !tile.biome.canAutoChoose)
					{
						return 0f;
					}
					return tile.biome.factionBaseSelectionWeight;
				}, out num))
				{
					if (!Find.WorldObjects.AnySettlementAt(num))
					{
						if (!Find.WorldObjects.AnyFactionBaseOrDestroyedFactionBaseAtOrAdjacent(num))
						{
							if (Current.Game.FindMap(num) == null)
							{
								if (!Find.WorldObjects.AnyMapParentAt(num))
								{
									return num;
								}
							}
						}
					}
				}
			}
			Log.Error("Failed to find faction base tile for " + faction);
			return 0;
		}

		public static bool TryFindPassableTileWithTraversalDistance(int rootTile, int minDist, int maxDist, out int result, Predicate<int> validator = null, bool ignoreFirstTilePassability = false)
		{
			TileFinder.tmpTiles.Clear();
			Find.WorldFloodFiller.FloodFill(rootTile, (int x) => !Find.World.Impassable(x) || (x == rootTile && ignoreFirstTilePassability), delegate(int tile, int traversalDistance)
			{
				if (traversalDistance > maxDist)
				{
					return true;
				}
				if (traversalDistance >= minDist && (validator == null || validator(tile)))
				{
					TileFinder.tmpTiles.Add(new Pair<int, int>(tile, traversalDistance));
				}
				return false;
			}, 2147483647);
			Pair<int, int> pair;
			if (TileFinder.tmpTiles.TryRandomElementByWeight((Pair<int, int> x) => 1f - (float)(x.Second - minDist) / ((float)(maxDist - minDist) + 0.01f), out pair))
			{
				result = pair.First;
				return true;
			}
			result = -1;
			return false;
		}

		public static bool TryFindRandomPlayerTile(out int tile)
		{
			Map map;
			if ((from x in Find.Maps
			where x.IsPlayerHome
			select x).TryRandomElement(out map))
			{
				tile = map.Tile;
				return true;
			}
			Caravan caravan;
			if ((from x in Find.WorldObjects.Caravans
			where x.IsPlayerControlled
			select x).TryRandomElement(out caravan))
			{
				tile = caravan.Tile;
				return true;
			}
			tile = -1;
			return false;
		}

		public static bool TryFindNewSiteTile(out int tile)
		{
			int rootTile;
			if (!TileFinder.TryFindRandomPlayerTile(out rootTile))
			{
				tile = -1;
				return false;
			}
			return TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, 8, 30, out tile, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x), false);
		}
	}
}
