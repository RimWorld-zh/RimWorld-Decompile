using System;

namespace Verse
{
	// Token: 0x02000C1E RID: 3102
	public class CellGrid
	{
		// Token: 0x060043C9 RID: 17353 RVA: 0x0023BAF0 File Offset: 0x00239EF0
		public CellGrid()
		{
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x0023BAF9 File Offset: 0x00239EF9
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A9F RID: 2719
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

		// Token: 0x17000AA0 RID: 2720
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

		// Token: 0x17000AA1 RID: 2721
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

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060043D1 RID: 17361 RVA: 0x0023BC20 File Offset: 0x0023A020
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x0023BC40 File Offset: 0x0023A040
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0023BC88 File Offset: 0x0023A088
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

		// Token: 0x060043D4 RID: 17364 RVA: 0x0023BD00 File Offset: 0x0023A100
		public void Clear()
		{
			int num = CellIndicesUtility.CellToIndex(IntVec3.Invalid, this.mapSizeX);
			for (int i = 0; i < this.grid.Length; i++)
			{
				this.grid[i] = num;
			}
		}

		// Token: 0x04002E4A RID: 11850
		private int[] grid;

		// Token: 0x04002E4B RID: 11851
		private int mapSizeX;

		// Token: 0x04002E4C RID: 11852
		private int mapSizeZ;
	}
}
