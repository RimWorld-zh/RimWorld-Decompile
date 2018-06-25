using System;

namespace Verse
{
	// Token: 0x02000C1D RID: 3101
	public class CellGrid
	{
		// Token: 0x04002E54 RID: 11860
		private int[] grid;

		// Token: 0x04002E55 RID: 11861
		private int mapSizeX;

		// Token: 0x04002E56 RID: 11862
		private int mapSizeZ;

		// Token: 0x060043D5 RID: 17365 RVA: 0x0023CF94 File Offset: 0x0023B394
		public CellGrid()
		{
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0023CF9D File Offset: 0x0023B39D
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000AA0 RID: 2720
		public IntVec3 this[IntVec3 c]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x17000AA1 RID: 2721
		public IntVec3 this[int index]
		{
			get
			{
				return CellIndicesUtility.IndexToCell(this.grid[index], this.mapSizeX);
			}
			set
			{
				this.grid[index] = CellIndicesUtility.CellToIndex(value, this.mapSizeX);
			}
		}

		// Token: 0x17000AA2 RID: 2722
		public IntVec3 this[int x, int z]
		{
			get
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				return CellIndicesUtility.IndexToCell(this.grid[num], this.mapSizeX);
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
				this.grid[num] = CellIndicesUtility.CellToIndex(x, z, this.mapSizeX);
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x0023D0C4 File Offset: 0x0023B4C4
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0023D0E4 File Offset: 0x0023B4E4
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0023D12C File Offset: 0x0023B52C
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
			}
			else
			{
				this.mapSizeX = map.Size.x;
				this.mapSizeZ = map.Size.z;
				this.grid = new int[this.mapSizeX * this.mapSizeZ];
				this.Clear();
			}
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0023D1A4 File Offset: 0x0023B5A4
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}
	}
}
