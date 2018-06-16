using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200056D RID: 1389
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x06001A43 RID: 6723 RVA: 0x000E35AC File Offset: 0x000E19AC
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

		// Token: 0x06001A44 RID: 6724 RVA: 0x000E36D8 File Offset: 0x000E1AD8
		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000E3720 File Offset: 0x000E1B20
		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}

		// Token: 0x04000F57 RID: 3927
		private List<int> group = new List<int>();

		// Token: 0x04000F58 RID: 3928
		private List<int> edgeTiles = new List<int>();
	}
}
