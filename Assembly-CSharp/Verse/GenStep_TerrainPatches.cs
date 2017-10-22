using UnityEngine;

namespace Verse
{
	public class GenStep_TerrainPatches : GenStep
	{
		public TerrainDef terrainDef;

		public FloatRange patchesPer10kCellsRange;

		public FloatRange patchSizeRange;

		public override void Generate(Map map)
		{
			int num = Mathf.RoundToInt((float)((float)map.Area / 10000.0 * this.patchesPer10kCellsRange.RandomInRange));
			for (int num2 = 0; num2 < num; num2++)
			{
				float randomInRange = this.patchSizeRange.RandomInRange;
				IntVec3 a = CellFinder.RandomCell(map);
				foreach (IntVec3 item in GenRadial.RadialPatternInRadius((float)(randomInRange / 2.0)))
				{
					IntVec3 c = a + item;
					if (c.InBounds(map))
					{
						map.terrainGrid.SetTerrain(c, this.terrainDef);
					}
				}
			}
		}
	}
}
