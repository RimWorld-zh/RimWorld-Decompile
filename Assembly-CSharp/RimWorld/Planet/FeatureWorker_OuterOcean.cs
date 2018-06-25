using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056B RID: 1387
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x04000F58 RID: 3928
		private List<int> group = new List<int>();

		// Token: 0x04000F59 RID: 3929
		private List<int> edgeTiles = new List<int>();

		// Token: 0x06001A3E RID: 6718 RVA: 0x000E3A0C File Offset: 0x000E1E0C
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

		// Token: 0x06001A3F RID: 6719 RVA: 0x000E3B38 File Offset: 0x000E1F38
		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000E3B80 File Offset: 0x000E1F80
		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}
	}
}
