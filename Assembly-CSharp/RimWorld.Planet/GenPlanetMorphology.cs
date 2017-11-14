using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public static class GenPlanetMorphology
	{
		private static HashSet<int> tmpOutput = new HashSet<int>();

		private static HashSet<int> tilesSet = new HashSet<int>();

		private static List<int> tmpNeighbors = new List<int>();

		private static List<int> tmpEdgeTiles = new List<int>();

		public static void Erode(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count > 0)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				GenPlanetMorphology.tilesSet.Clear();
				GenPlanetMorphology.tilesSet.AddRange(tiles);
				GenPlanetMorphology.tmpEdgeTiles.Clear();
				for (int i = 0; i < tiles.Count; i++)
				{
					worldGrid.GetTileNeighbors(tiles[i], GenPlanetMorphology.tmpNeighbors);
					int num = 0;
					while (num < GenPlanetMorphology.tmpNeighbors.Count)
					{
						if (GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[num]))
						{
							num++;
							continue;
						}
						GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
						break;
					}
				}
				if (GenPlanetMorphology.tmpEdgeTiles.Any())
				{
					GenPlanetMorphology.tmpOutput.Clear();
					Predicate<int> predicate = (extraPredicate == null) ? ((Predicate<int>)((int x) => GenPlanetMorphology.tilesSet.Contains(x))) : ((Predicate<int>)((int x) => GenPlanetMorphology.tilesSet.Contains(x) && extraPredicate(x)));
					WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
					int rootTile = -1;
					Predicate<int> passCheck = predicate;
					Func<int, int, bool> processor = delegate(int tile, int traversalDist)
					{
						if (traversalDist >= count)
						{
							GenPlanetMorphology.tmpOutput.Add(tile);
						}
						return false;
					};
					List<int> extraRootTiles = GenPlanetMorphology.tmpEdgeTiles;
					worldFloodFiller.FloodFill(rootTile, passCheck, processor, 2147483647, extraRootTiles);
					tiles.Clear();
					tiles.AddRange(GenPlanetMorphology.tmpOutput);
				}
			}
		}

		public static void Dilate(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count > 0)
			{
				WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
				int rootTile = -1;
				Predicate<int> passCheck = extraPredicate ?? ((Predicate<int>)((int x) => true));
				Func<int, int, bool> processor = delegate(int tile, int traversalDist)
				{
					if (traversalDist > count)
					{
						return true;
					}
					if (traversalDist != 0)
					{
						tiles.Add(tile);
					}
					return false;
				};
				List<int> extraRootTiles = tiles;
				worldFloodFiller.FloodFill(rootTile, passCheck, processor, 2147483647, extraRootTiles);
			}
		}

		public static void Open(List<int> tiles, int count)
		{
			GenPlanetMorphology.Erode(tiles, count, null);
			GenPlanetMorphology.Dilate(tiles, count, null);
		}

		public static void Close(List<int> tiles, int count)
		{
			GenPlanetMorphology.Dilate(tiles, count, null);
			GenPlanetMorphology.Erode(tiles, count, null);
		}
	}
}
