using System;

namespace Verse
{
	// Token: 0x02000C62 RID: 3170
	public class MapGenFloatGrid
	{
		// Token: 0x060045A3 RID: 17827 RVA: 0x0024BAC3 File Offset: 0x00249EC3
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AFE RID: 2814
		public float this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x04002F9A RID: 12186
		private Map map;

		// Token: 0x04002F9B RID: 12187
		private float[] grid;
	}
}
