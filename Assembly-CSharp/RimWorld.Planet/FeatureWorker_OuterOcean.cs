using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		private List<int> group = new List<int>();

		private List<int> edgeTiles = new List<int>();

		public override void GenerateWhereAppropriate()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			this.edgeTiles.Clear();
			for (int num = 0; num < tilesCount; num++)
			{
				if (this.IsRoot(num))
				{
					this.edgeTiles.Add(num);
				}
			}
			if (this.edgeTiles.Any())
			{
				this.group.Clear();
				WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
				int rootTile = -1;
				Predicate<int> passCheck = (Predicate<int>)((int x) => this.CanTraverse(x));
				Func<int, int, bool> processor = (Func<int, int, bool>)delegate(int tile, int traversalDist)
				{
					this.group.Add(tile);
					return false;
				};
				List<int> extraRootTiles = this.edgeTiles;
				worldFloodFiller.FloodFill(rootTile, passCheck, processor, 2147483647, extraRootTiles);
				this.group.RemoveAll((Predicate<int>)((int x) => worldGrid[x].feature != null));
				if (this.group.Count >= base.def.minSize && this.group.Count <= base.def.maxSize)
				{
					base.AddFeature(this.group, this.group);
				}
			}
		}

		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
