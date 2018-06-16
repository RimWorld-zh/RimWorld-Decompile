using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005AB RID: 1451
	public static class TileFinder
	{
		// Token: 0x06001B9E RID: 7070 RVA: 0x000EE2E4 File Offset: 0x000EC6E4
		public static int RandomStartingTile()
		{
			return TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, true, null);
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000EE308 File Offset: 0x000EC708
		public static int RandomFactionBaseTileFor(Faction faction, bool mustBeAutoChoosable = false, Predicate<int> extraValidator = null)
		{
			for (int i = 0; i < 500; i++)
			{
				int num;
				if ((from _ in Enumerable.Range(0, 100)
				select Rand.Range(0, Find.WorldGrid.TilesCount)).TryRandomElementByWeight(delegate(int x)
				{
					Tile tile = Find.WorldGrid[x];
					float result;
					if (!tile.biome.canBuildBase || !tile.biome.implemented || tile.hilliness == Hilliness.Impassable)
					{
						result = 0f;
					}
					else if (mustBeAutoChoosable && !tile.biome.canAutoChoose)
					{
						result = 0f;
					}
					else if (extraValidator != null && !extraValidator(x))
					{
						result = 0f;
					}
					else
					{
						result = tile.biome.factionBaseSelectionWeight;
					}
					return result;
				}, out num))
				{
					if (TileFinder.IsValidTileForNewSettlement(num, null))
					{
						return num;
					}
				}
			}
			Log.Error("Failed to find faction base tile for " + faction, false);
			return 0;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000EE3B8 File Offset: 0x000EC7B8
		public static bool IsValidTileForNewSettlement(int tile, StringBuilder reason = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			bool result;
			if (!tile2.biome.canBuildBase)
			{
				if (reason != null)
				{
					reason.Append("CannotLandBiome".Translate(new object[]
					{
						tile2.biome.LabelCap
					}));
				}
				result = false;
			}
			else if (!tile2.biome.implemented)
			{
				if (reason != null)
				{
					reason.Append("BiomeNotImplemented".Translate() + ": " + tile2.biome.LabelCap);
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
							reason.Append("BaseAlreadyThere".Translate(new object[]
							{
								settlement.Faction.Name
							}));
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
				else if (Find.WorldObjects.AnyMapParentAt(tile) || Current.Game.FindMap(tile) != null || Find.WorldObjects.AnyWorldObjectOfDefAt(WorldObjectDefOf.AbandonedBase, tile))
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

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000EE5A0 File Offset: 0x000EC9A0
		public static bool TryFindPassableTileWithTraversalDistance(int rootTile, int minDist, int maxDist, out int result, Predicate<int> validator = null, bool ignoreFirstTilePassability = false, bool preferCloserTiles = false)
		{
			TileFinder.tmpTiles.Clear();
			Find.WorldFloodFiller.FloodFill(rootTile, (int x) => !Find.World.Impassable(x) || (x == rootTile && ignoreFirstTilePassability), delegate(int tile, int traversalDistance)
			{
				bool result3;
				if (traversalDistance > maxDist)
				{
					result3 = true;
				}
				else
				{
					if (traversalDistance >= minDist && (validator == null || validator(tile)))
					{
						TileFinder.tmpTiles.Add(new Pair<int, int>(tile, traversalDistance));
					}
					result3 = false;
				}
				return result3;
			}, int.MaxValue, null);
			Pair<int, int> pair;
			bool result2;
			if (preferCloserTiles)
			{
				if (TileFinder.tmpTiles.TryRandomElementByWeight((Pair<int, int> x) => 1f - (float)(x.Second - minDist) / ((float)(maxDist - minDist) + 0.01f), out pair))
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
			else if (TileFinder.tmpTiles.TryRandomElement(out pair))
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

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000EE684 File Offset: 0x000ECA84
		public static bool TryFindRandomPlayerTile(out int tile, bool allowCaravans, Predicate<int> validator = null)
		{
			TileFinder.tmpPlayerTiles.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapPawns.FreeColonistsSpawnedCount != 0 && (validator == null || validator(maps[i].Tile)))
				{
					TileFinder.tmpPlayerTiles.Add(maps[i].Tile);
				}
			}
			if (allowCaravans)
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled && (validator == null || validator(caravans[j].Tile)))
					{
						TileFinder.tmpPlayerTiles.Add(caravans[j].Tile);
					}
				}
			}
			bool result;
			Map map;
			Map map2;
			if (TileFinder.tmpPlayerTiles.TryRandomElement(out tile))
			{
				result = true;
			}
			else if ((from x in Find.Maps
			where x.IsPlayerHome && (validator == null || validator(x.Tile))
			select x).TryRandomElement(out map))
			{
				tile = map.Tile;
				result = true;
			}
			else if ((from x in Find.Maps
			where x.mapPawns.FreeColonistsSpawnedCount != 0 && (validator == null || validator(x.Tile))
			select x).TryRandomElement(out map2))
			{
				tile = map2.Tile;
				result = true;
			}
			else
			{
				if (!allowCaravans)
				{
					Caravan caravan;
					if ((from x in Find.WorldObjects.Caravans
					where x.IsPlayerControlled && (validator == null || validator(x.Tile))
					select x).TryRandomElement(out caravan))
					{
						tile = caravan.Tile;
						return true;
					}
				}
				tile = -1;
				result = false;
			}
			return result;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000EE880 File Offset: 0x000ECC80
		public static bool TryFindNewSiteTile(out int tile, int minDist = 7, int maxDist = 27, bool allowCaravans = false, bool preferCloserTiles = true, int nearThisTile = -1)
		{
			Func<int, int> findTile = delegate(int root)
			{
				int minDist2 = minDist;
				int maxDist2 = maxDist;
				int num;
				ref int result = ref num;
				Predicate<int> validator = (int x) => !Find.WorldObjects.AnyWorldObjectAt(x) && TileFinder.IsValidTileForNewSettlement(x, null);
				bool preferCloserTiles2 = preferCloserTiles;
				int result2;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist2, maxDist2, out result, validator, false, preferCloserTiles2))
				{
					result2 = num;
				}
				else
				{
					result2 = -1;
				}
				return result2;
			};
			int arg;
			if (nearThisTile != -1)
			{
				arg = nearThisTile;
			}
			else if (!TileFinder.TryFindRandomPlayerTile(out arg, allowCaravans, (int x) => findTile(x) != -1))
			{
				tile = -1;
				return false;
			}
			tile = findTile(arg);
			return tile != -1;
		}

		// Token: 0x04001071 RID: 4209
		private static List<Pair<int, int>> tmpTiles = new List<Pair<int, int>>();

		// Token: 0x04001072 RID: 4210
		private static List<int> tmpPlayerTiles = new List<int>();
	}
}
