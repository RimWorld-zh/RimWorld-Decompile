using System;

namespace Verse
{
	// Token: 0x02000C5E RID: 3166
	public class MapGenFloatGrid
	{
		// Token: 0x04002FA2 RID: 12194
		private Map map;

		// Token: 0x04002FA3 RID: 12195
		private float[] grid;

		// Token: 0x060045AA RID: 17834 RVA: 0x0024CE6B File Offset: 0x0024B26B
		public MapGenFloatGrid(Map map)
		{
			this.map = map;
			this.grid = new float[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AFF RID: 2815
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
