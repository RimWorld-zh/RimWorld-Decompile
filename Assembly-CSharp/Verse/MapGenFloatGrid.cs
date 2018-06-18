using System;

namespace Verse
{
	// Token: 0x02000C61 RID: 3169
	public class MapGenFloatGrid
	{
		// Token: 0x060045A1 RID: 17825 RVA: 0x0024BA9B File Offset: 0x00249E9B
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AFD RID: 2813
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

		// Token: 0x04002F98 RID: 12184
		private Map map;

		// Token: 0x04002F99 RID: 12185
		private float[] grid;
	}
}
