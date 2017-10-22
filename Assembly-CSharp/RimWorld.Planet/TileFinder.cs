using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				int num = default(int);
				if ((from _ in Enumerable.Range(0, 100)
				select Rand.Range(0, Find.WorldGrid.TilesCount)).TryRandomElementByWeight<int>((Func<int, float>)delegate(int x)
				{
					Tile tile = Find.WorldGrid[x];
					if (tile.biome.canBuildBase && tile.biome.implemented && tile.hilliness != Hilliness.Impassable)
					{
						if (mustBeAutoChoosable && !tile.biome.canAutoChoose)
						{
							return 0f;
						}
						return tile.biome.factionBaseSelectionWeight;
					}
					return 0f;
				}, out num) && TileFinder.IsValidTileForNewSettlement(num, null))
				{
					return num;
				}
			}
			Log.Error("Failed to find faction base tile for " + faction);
			return 0;
		}

		public static bool IsValidTileForNewSettlement(int tile, StringBuilder reason = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (!tile2.biome.canBuildBase)
			{
				if (reason != null)
				{
					reason.Append("CannotLandBiome".Translate(tile2.biome.label));
				}
				return false;
			}
			if (!tile2.biome.implemented)
			{
				if (reason != null)
				{
					reason.Append("BiomeNotImplemented".Translate() + ": " + tile2.biome.label);
				}
				return false;
			}
			if (tile2.hilliness == Hilliness.Impassable)
			{
				if (reason != null)
				{
					reason.Append("CannotLandImpassableMountains".Translate());
				}
				return false;
			}
			Settlement settlement = Find.WorldObjects.SettlementAt(tile);
			if (settlement != null)
			{
				if (reason != null)
				{
					if (settlement.Faction == null)
					{
						reason.Append("TileOccupied".Translate());
					}
					else if (settlement.Faction == Faction.OfPlayer)
					{
						reason.Append("YourBaseAlreadyThere".Translate());
					}
					else
					{
						reason.Append("BaseAlreadyThere".Translate(settlement.Faction.Name));
					}
				}
				return false;
			}
			if (Find.WorldObjects.AnySettlementAtOrAdjacent(tile))
			{
				if (reason != null)
				{
					reason.Append("FactionBaseAdjacent".Translate());
				}
				return false;
			}
			if (!Find.WorldObjects.AnyMapParentAt(tile) && Current.Game.FindMap(tile) == null)
			{
				return true;
			}
			if (reason != null)
			{
				reason.Append("TileOccupied".Translate());
			}
			return false;
		}

		public static bool TryFindPassableTileWithTraversalDistance(int rootTile, int minDist, int maxDist, out int result, Predicate<int> validator = null, bool ignoreFirstTilePassability = false)
		{
			TileFinder.tmpTiles.Clear();
			Find.WorldFloodFiller.FloodFill(rootTile, (Predicate<int>)((int x) => !Find.World.Impassable(x) || (x == rootTile && ignoreFirstTilePassability)), (Func<int, int, bool>)delegate(int tile, int traversalDistance)
			{
				if (traversalDistance > maxDist)
				{
					return true;
				}
				if (traversalDistance >= minDist && ((object)validator == null || validator(tile)))
				{
					TileFinder.tmpTiles.Add(new Pair<int, int>(tile, traversalDistance));
				}
				return false;
			}, 2147483647);
			Pair<int, int> pair = default(Pair<int, int>);
			if (((IEnumerable<Pair<int, int>>)TileFinder.tmpTiles).TryRandomElementByWeight<Pair<int, int>>((Func<Pair<int, int>, float>)((Pair<int, int> x) => (float)(1.0 - (float)(x.Second - minDist) / (float)((float)(maxDist - minDist) + 0.0099999997764825821))), out pair))
			{
				result = pair.First;
				return true;
			}
			result = -1;
			return false;
		}

		public static bool TryFindRandomPlayerTile(out int tile)
		{
			Map map = default(Map);
			if ((from x in Find.Maps
			where x.IsPlayerHome && x.mapPawns.FreeColonistsSpawnedCount != 0
			select x).TryRandomElement<Map>(out map))
			{
				tile = map.Tile;
				return true;
			}
			if ((from x in Find.Maps
			where x.IsPlayerHome
			select x).TryRandomElement<Map>(out map))
			{
				tile = map.Tile;
				return true;
			}
			Caravan caravan = default(Caravan);
			if ((from x in Find.WorldObjects.Caravans
			where x.IsPlayerControlled
			select x).TryRandomElement<Caravan>(out caravan))
			{
				tile = caravan.Tile;
				return true;
			}
			tile = -1;
			return false;
		}

		public static bool TryFindNewSiteTile(out int tile)
		{
			int rootTile = default(int);
			if (!TileFinder.TryFindRandomPlayerTile(out rootTile))
			{
				tile = -1;
				return false;
			}
			return TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, 8, 30, out tile, (Predicate<int>)((int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && TileFinder.IsValidTileForNewSettlement(x, null)), false);
		}
	}
}
