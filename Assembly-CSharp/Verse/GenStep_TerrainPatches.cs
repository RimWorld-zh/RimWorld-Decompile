using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C5C RID: 3164
	public class GenStep_TerrainPatches : GenStep
	{
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x0024B09C File Offset: 0x0024949C
		public override int SeedPart
		{
			get
			{
				return 1370184742;
			}
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x0024B0B8 File Offset: 0x002494B8
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

		// Token: 0x04002F7B RID: 12155
		public TerrainDef terrainDef;

		// Token: 0x04002F7C RID: 12156
		public FloatRange patchesPer10kCellsRange;

		// Token: 0x04002F7D RID: 12157
		public FloatRange patchSizeRange;
	}
}
