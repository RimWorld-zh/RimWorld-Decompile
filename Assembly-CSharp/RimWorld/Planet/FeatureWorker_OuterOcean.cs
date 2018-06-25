using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056B RID: 1387
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x04000F54 RID: 3924
		private List<int> group = new List<int>();

		// Token: 0x04000F55 RID: 3925
		private List<int> edgeTiles = new List<int>();

		// Token: 0x06001A3F RID: 6719 RVA: 0x000E37A4 File Offset: 0x000E1BA4
		public override void GenerateWhereAppropriate()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			this.edgeTiles.Clear();
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.edgeTiles.Add(i);
				}
			}
			if (this.edgeTiles.Any<int>())
			{
				this.group.Clear();
				WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
				int rootTile = -1;
				Predicate<int> passCheck = (int x) => this.CanTraverse(x);
				Func<int, int, bool> processor = delegate(int tile, int traversalDist)
				{
					this.group.Add(tile);
					return false;
				};
				List<int> extraRootTiles = this.edgeTiles;
				worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, extraRootTiles);
				this.group.RemoveAll((int x) => worldGrid[x].feature != null);
				if (this.group.Count >= this.def.minSize && this.group.Count <= this.def.maxSize)
				{
					base.AddFeature(this.group, this.group);
				}
			}
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000E38D0 File Offset: 0x000E1CD0
		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000E3918 File Offset: 0x000E1D18
		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
