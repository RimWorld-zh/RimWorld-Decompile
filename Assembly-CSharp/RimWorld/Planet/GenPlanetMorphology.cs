using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public static class GenPlanetMorphology
	{
		private static HashSet<int> tmpOutput = new HashSet<int>();

		private static HashSet<int> tilesSet = new HashSet<int>();

		private static List<int> tmpNeighbors = new List<int>();

		private static List<int> tmpEdgeTiles = new List<int>();

		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<int> <>f__am$cache1;

		public static void Erode(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			GenPlanetMorphology.tilesSet.Clear();
			GenPlanetMorphology.tilesSet.AddRange(tiles);
			GenPlanetMorphology.tmpEdgeTiles.Clear();
			for (int i = 0; i < tiles.Count; i++)
			{
				worldGrid.GetTileNeighbors(tiles[i], GenPlanetMorphology.tmpNeighbors);
				for (int j = 0; j < GenPlanetMorphology.tmpNeighbors.Count; j++)
				{
					if (!GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[j]))
					{
						GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
						break;
					}
				}
			}
			if (!GenPlanetMorphology.tmpEdgeTiles.Any<int>())
			{
				return;
			}
			GenPlanetMorphology.tmpOutput.Clear();
			Predicate<int> predicate;
			if (extraPredicate != null)
			{
				predicate = ((int x) => GenPlanetMorphology.tilesSet.Contains(x) && extraPredicate(x));
			}
			else
			{
				predicate = ((int x) => GenPlanetMorphology.tilesSet.Contains(x));
			}
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
			worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, extraRootTiles);
			tiles.Clear();
			tiles.AddRange(GenPlanetMorphology.tmpOutput);
		}

		public static void Dilate(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count <= 0)
			{
				return;
			}
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int rootTile = -1;
			Predicate<int> predicate = extraPredicate;
			if (extraPredicate == null)
			{
				predicate = ((int x) => true);
			}
			Predicate<int> passCheck = predicate;
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
			List<int> tiles2 = tiles;
			worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, tiles2);
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

		// Note: this type is marked as 'beforefieldinit'.
		static GenPlanetMorphology()
		{
		}

		[CompilerGenerated]
		private static bool <Erode>m__0(int x)
		{
			return GenPlanetMorphology.tilesSet.Contains(x);
		}

		[CompilerGenerated]
		private static bool <Dilate>m__1(int x)
		{
			return true;
		}

		[CompilerGenerated]
		private sealed class <Erode>c__AnonStorey0
		{
			internal Predicate<int> extraPredicate;

			internal int count;

			public <Erode>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int x)
			{
				return GenPlanetMorphology.tilesSet.Contains(x) && this.extraPredicate(x);
			}

			internal bool <>m__1(int tile, int traversalDist)
			{
				if (traversalDist >= this.count)
				{
					GenPlanetMorphology.tmpOutput.Add(tile);
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <Dilate>c__AnonStorey1
		{
			internal int count;

			internal List<int> tiles;

			public <Dilate>c__AnonStorey1()
			{
			}

			internal bool <>m__0(int tile, int traversalDist)
			{
				if (traversalDist > this.count)
				{
					return true;
				}
				if (traversalDist != 0)
				{
					this.tiles.Add(tile);
				}
				return false;
			}
		}
	}
}
