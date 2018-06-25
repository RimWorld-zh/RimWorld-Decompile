using System;

namespace Verse
{
	// Token: 0x02000C61 RID: 3169
	public class MapGenFloatGrid
	{
		// Token: 0x04002FA9 RID: 12201
		private Map map;

		// Token: 0x04002FAA RID: 12202
		private float[] grid;

		// Token: 0x060045AD RID: 17837 RVA: 0x0024D227 File Offset: 0x0024B627
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
	}
}
