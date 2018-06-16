using System;

namespace Verse
{
	// Token: 0x02000C1F RID: 3103
	public class CellGrid
	{
		// Token: 0x060043CB RID: 17355 RVA: 0x0023BB18 File Offset: 0x00239F18
		public CellGrid()
		{
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x0023BB21 File Offset: 0x00239F21
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
		// (get) Token: 0x060043D3 RID: 17363 RVA: 0x0023BC48 File Offset: 0x0023A048
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0023BC68 File Offset: 0x0023A068
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0023BCB0 File Offset: 0x0023A0B0
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

		// Token: 0x060043D6 RID: 17366 RVA: 0x0023BD28 File Offset: 0x0023A128
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x04002E4C RID: 11852
		private int[] grid;

		// Token: 0x04002E4D RID: 11853
		private int mapSizeX;

		// Token: 0x04002E4E RID: 11854
		private int mapSizeZ;
	}
}
