using System;

namespace Verse
{
	// Token: 0x02000C1B RID: 3099
	public class CellGrid
	{
		// Token: 0x04002E54 RID: 11860
		private int[] grid;

		// Token: 0x04002E55 RID: 11861
		private int mapSizeX;

		// Token: 0x04002E56 RID: 11862
		private int mapSizeZ;

		// Token: 0x060043D2 RID: 17362 RVA: 0x0023CEB8 File Offset: 0x0023B2B8
		public CellGrid()
		{
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0023CEC1 File Offset: 0x0023B2C1
		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000AA1 RID: 2721
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

		// Token: 0x17000AA2 RID: 2722
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

		// Token: 0x17000AA3 RID: 2723
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

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060043DA RID: 17370 RVA: 0x0023CFE8 File Offset: 0x0023B3E8
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x0023D008 File Offset: 0x0023B408
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x0023D050 File Offset: 0x0023B450
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

		// Token: 0x060043DD RID: 17373 RVA: 0x0023D0C8 File Offset: 0x0023B4C8
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
