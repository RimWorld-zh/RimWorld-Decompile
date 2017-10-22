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

		private static List<int> tmpPlayerTiles = new List<int>();

		public static int RandomStartingTile()
		{
			return TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, true, null);
		}

		public static int RandomFactionBaseTileFor(Faction faction, bool mustBeAutoChoosable = false, Predicate<int> extraValidator = null)
		{
			int num = 0;
			int result;
			while (true)
			{
				if (num < 500)
				{
					int num2 = default(int);
					if ((from _ in Enumerable.Range(0, 100)
					select Rand.Range(0, Find.WorldGrid.TilesCount)).TryRandomElementByWeight<int>((Func<int, float>)delegate(int x)
					{
						Tile tile = Find.WorldGrid[x];
						return (float)((!tile.biome.canBuildBase || !tile.biome.implemented || tile.hilliness == Hilliness.Impassable) ? 0.0 : ((!mustBeAutoChoosable || tile.biome.canAutoChoose) ? (((object)extraValidator == null || extraValidator(x)) ? tile.biome.factionBaseSelectionWeight : 0.0) : 0.0));
					}, out num2) && TileFinder.IsValidTileForNewSettlement(num2, null))
					{
						result = num2;
						break;
					}
					num++;
					continue;
				}
				Log.Error("Failed to find faction base tile for " + faction);
				result = 0;
				break;
			}
			return result;
		}

		public static bool IsValidTileForNewSettlement(int tile, StringBuilder reason = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			bool result;
			if (!tile2.biome.canBuildBase)
			{
				if (reason != null)
				{
					reason.Append("CannotLandBiome".Translate(tile2.biome.label));
				}
				result = false;
			}
			else if (!tile2.biome.implemented)
			{
				if (reason != null)
				{
					reason.Append("BiomeNotImplemented".Translate() + ": " + tile2.biome.label);
				}
				result = false;
			}
			else if (tile2.hilliness == Hilliness.Impassable)
			{
				if (reason != null)
				{
					reason.Append("CannotLandImpassableMountains".Translate());
				}
				result = false;
			}
			else
			{
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
					result = false;
				}
				else if (Find.WorldObjects.AnySettlementAtOrAdjacent(tile))
				{
					if (reason != null)
					{
						reason.Append("FactionBaseAdjacent".Translate());
					}
					result = false;
				}
				else if (Find.WorldObjects.AnyMapParentAt(tile) || Current.Game.FindMap(tile) != null || Find.WorldObjects.AnyWorldObjectOfDefAt(WorldObjectDefOf.AbandonedFactionBase, tile))
				{
					if (reason != null)
					{
						reason.Append("TileOccupied".Translate());
					}
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public static bool TryFindPassableTileWithTraversalDistance(int rootTile, int minDist, int maxDist, out int result, Predicate<int> validator = null, bool ignoreFirstTilePassability = false, bool preferCloserTiles = false)
		{
			TileFinder.tmpTiles.Clear();
			Find.WorldFloodFiller.FloodFill(rootTile, (Predicate<int>)((int x) => !Find.World.Impassable(x) || (x == rootTile && ignoreFirstTilePassability)), (Func<int, int, bool>)delegate(int tile, int traversalDistance)
			{
				bool result3;
				if (traversalDistance > maxDist)
				{
					result3 = true;
				}
				else
				{
					if (traversalDistance >= minDist && ((object)validator == null || validator(tile)))
					{
						TileFinder.tmpTiles.Add(new Pair<int, int>(tile, traversalDistance));
					}
					result3 = false;
				}
				return result3;
			}, 2147483647, null);
			Pair<int, int> pair = default(Pair<int, int>);
			bool result2;
			if (preferCloserTiles)
			{
				if (((IEnumerable<Pair<int, int>>)TileFinder.tmpTiles).TryRandomElementByWeight<Pair<int, int>>((Func<Pair<int, int>, float>)((Pair<int, int> x) => (float)(1.0 - (float)(x.Second - minDist) / (float)((float)(maxDist - minDist) + 0.0099999997764825821))), out pair))
				{
					result = pair.First;
					result2 = true;
				}
				else
				{
					result = -1;
					result2 = false;
				}
			}
			else if (((IEnumerable<Pair<int, int>>)TileFinder.tmpTiles).TryRandomElement<Pair<int, int>>(out pair))
			{
				result = pair.First;
				result2 = true;
			}
			else
			{
				result = -1;
				result2 = false;
			}
			return result2;
		}

		public static bool TryFindRandomPlayerTile(out int tile, bool allowCaravans, Predicate<int> validator = null)
		{
			TileFinder.tmpPlayerTiles.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapPawns.FreeColonistsSpawnedCount != 0 && ((object)validator == null || validator(maps[i].Tile)))
				{
					TileFinder.tmpPlayerTiles.Add(maps[i].Tile);
				}
			}
			if (allowCaravans)
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled && ((object)validator == null || validator(caravans[j].Tile)))
					{
						TileFinder.tmpPlayerTiles.Add(caravans[j].Tile);
					}
				}
			}
			bool result;
			Map map = default(Map);
			Map map2 = default(Map);
			Caravan caravan = default(Caravan);
			if (((IEnumerable<int>)TileFinder.tmpPlayerTiles).TryRandomElement<int>(out tile))
			{
				result = true;
			}
			else if ((from x in Find.Maps
			where x.IsPlayerHome && ((object)validator == null || validator(x.Tile))
			select x).TryRandomElement<Map>(out map))
			{
				tile = map.Tile;
				result = true;
			}
			else if ((from x in Find.Maps
			where x.mapPawns.FreeColonistsSpawnedCount != 0 && ((object)validator == null || validator(x.Tile))
			select x).TryRandomElement<Map>(out map2))
			{
				tile = map2.Tile;
				result = true;
			}
			else if (!allowCaravans && (from x in Find.WorldObjects.Caravans
			where x.IsPlayerControlled && ((object)validator == null || validator(x.Tile))
			select x).TryRandomElement<Caravan>(out caravan))
			{
				tile = caravan.Tile;
				result = true;
			}
			else
			{
				tile = -1;
				result = false;
			}
			return result;
		}

		public static bool TryFindNewSiteTile(out int tile, int minDist = 8, int maxDist = 30, bool allowCaravans = false, bool preferCloserTiles = true, int nearThisTile = -1)
		{
			Func<int, int> findTile = (Func<int, int>)delegate(int root)
			{
				int minDist2 = minDist;
				int maxDist2 = maxDist;
				Predicate<int> validator = (Predicate<int>)((int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && TileFinder.IsValidTileForNewSettlement(x, null));
				bool preferCloserTiles2 = preferCloserTiles;
				int num = default(int);
				return (!TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist2, maxDist2, out num, validator, false, preferCloserTiles2)) ? (-1) : num;
			};
			int arg = default(int);
			bool result;
			if (nearThisTile != -1)
			{
				arg = nearThisTile;
			}
			else if (!TileFinder.TryFindRandomPlayerTile(out arg, allowCaravans, (Predicate<int>)((int x) => findTile(x) != -1)))
			{
				tile = -1;
				result = false;
				goto IL_0081;
			}
			tile = findTile(arg);
			result = (tile != -1);
			goto IL_0081;
			IL_0081:
			return result;
		}
	}
}
