using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005AD RID: 1453
	public static class GenPlanetMorphology
	{
		// Token: 0x04001090 RID: 4240
		private static HashSet<int> tmpOutput = new HashSet<int>();

		// Token: 0x04001091 RID: 4241
		private static HashSet<int> tilesSet = new HashSet<int>();

		// Token: 0x04001092 RID: 4242
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04001093 RID: 4243
		private static List<int> tmpEdgeTiles = new List<int>();

		// Token: 0x06001BCA RID: 7114 RVA: 0x000EFA08 File Offset: 0x000EDE08
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

		// Token: 0x06001BCB RID: 7115 RVA: 0x000EFB78 File Offset: 0x000EDF78
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

		// Token: 0x06001BCC RID: 7116 RVA: 0x000EFBFC File Offset: 0x000EDFFC
		public static void Open(List<int> tiles, int count)
		{
			GenPlanetMorphology.Erode(tiles, count, null);
			GenPlanetMorphology.Dilate(tiles, count, null);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000EFC0F File Offset: 0x000EE00F
		public static void Close(List<int> tiles, int count)
		{
			GenPlanetMorphology.Dilate(tiles, count, null);
			GenPlanetMorphology.Erode(tiles, count, null);
		}
	}
}
