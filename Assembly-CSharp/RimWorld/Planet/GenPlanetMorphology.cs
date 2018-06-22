using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005AB RID: 1451
	public static class GenPlanetMorphology
	{
		// Token: 0x06001BC7 RID: 7111 RVA: 0x000EF650 File Offset: 0x000EDA50
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
					for (int j = 0; j < GenPlanetMorphology.tmpNeighbors.Count; j++)
					{
						if (!GenPlanetMorphology.tilesSet.Contains(GenPlanetMorphology.tmpNeighbors[j]))
						{
							GenPlanetMorphology.tmpEdgeTiles.Add(tiles[i]);
							break;
						}
					}
				}
				if (GenPlanetMorphology.tmpEdgeTiles.Any<int>())
				{
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
			}
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000EF7C0 File Offset: 0x000EDBC0
		public static void Dilate(List<int> tiles, int count, Predicate<int> extraPredicate = null)
		{
			if (count > 0)
			{
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
					bool result;
					if (traversalDist > count)
					{
						result = true;
					}
					else
					{
						if (traversalDist != 0)
						{
							tiles.Add(tile);
						}
						result = false;
					}
					return result;
				};
				List<int> tiles2 = tiles;
				worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, tiles2);
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000EF844 File Offset: 0x000EDC44
		public static void Open(List<int> tiles, int count)
		{
			GenPlanetMorphology.Erode(tiles, count, null);
			GenPlanetMorphology.Dilate(tiles, count, null);
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000EF857 File Offset: 0x000EDC57
		public static void Close(List<int> tiles, int count)
		{
			GenPlanetMorphology.Dilate(tiles, count, null);
			GenPlanetMorphology.Erode(tiles, count, null);
		}

		// Token: 0x0400108C RID: 4236
		private static HashSet<int> tmpOutput = new HashSet<int>();

		// Token: 0x0400108D RID: 4237
		private static HashSet<int> tilesSet = new HashSet<int>();

		// Token: 0x0400108E RID: 4238
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x0400108F RID: 4239
		private static List<int> tmpEdgeTiles = new List<int>();
	}
}
