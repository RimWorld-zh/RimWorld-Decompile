using System;

namespace Verse
{
	// Token: 0x02000C60 RID: 3168
	public class MapGenFloatGrid
	{
		// Token: 0x04002FA2 RID: 12194
		private Map map;

		// Token: 0x04002FA3 RID: 12195
		private float[] grid;

		// Token: 0x060045AD RID: 17837 RVA: 0x0024CF47 File Offset: 0x0024B347
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
