using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C5B RID: 3163
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x0024B074 File Offset: 0x00249474
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x0024B090 File Offset: 0x00249490
		public override void Generate(Map map)
		{
			int num = Mathf.RoundToInt((float)map.Area / 10000f * this.patchesPer10kCellsRange.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				float randomInRange = this.patchSizeRange.RandomInRange;
				IntVec3 a = CellFinder.RandomCell(map);
				foreach (IntVec3 b in GenRadial.RadialPatternInRadius(randomInRange / 2f))
				{
					IntVec3 c = a + b;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrain(c, this.terrainDef);
					}
				}
			}
		}

		// Token: 0x04002F79 RID: 12153
		public TerrainDef terrainDef;

		// Token: 0x04002F7A RID: 12154
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04002F7B RID: 12155
		public FloatRange patchSizeRange;
	}
}
