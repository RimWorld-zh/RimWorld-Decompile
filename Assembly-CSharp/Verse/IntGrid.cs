using System;

namespace Verse
{
	// Token: 0x02000C25 RID: 3109
	public sealed class IntGrid
	{
		// Token: 0x0600441E RID: 17438 RVA: 0x0023D1EE File Offset: 0x0023B5EE
		public IntGrid()
		{
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x0023D1F7 File Offset: 0x0023B5F7
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000AAD RID: 2733
		public int this[IntVec3 c]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				int num = CellIndicesUtility.CellToIndex(c, this.mapSizeX);
				this.grid[num] = value;
			}
		}

		// Token: 0x17000AAE RID: 2734
		public int this[int index]
		{
			get
			{
				return this.grid[index];
			}
			set
			{
				this.grid[index] = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		public int this[int x, int z]
		{
			get
			{
				return this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.grid[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)] = value;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06004426 RID: 17446 RVA: 0x0023D2C4 File Offset: 0x0023B6C4
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x0023D2E4 File Offset: 0x0023B6E4
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0023D32C File Offset: 0x0023B72C
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear(0);
			}
			else
			{
				this.mapSizeX = map.Size.x;
				this.mapSizeZ = map.Size.z;
				this.grid = new int[this.mapSizeX * this.mapSizeZ];
			}
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0023D3A0 File Offset: 0x0023B7A0
		public void Clear(int value = 0)
		{
			if (value == 0)
			{
				Array.Clear(this.grid, 0, this.grid.Length);
			}
			else
			{
				for (int i = 0; i < this.grid.Length; i++)
				{
					this.grid[i] = value;
				}
			}
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0023D3F4 File Offset: 0x0023B7F4
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				int num = this.grid[i];
				if (num > 0)
				{
					IntVec3 c = CellIndicesUtility.IndexToCell(i, this.mapSizeX);
					CellRenderer.RenderCell(c, (float)(num % 100) / 100f * 0.5f);
				}
			}
		}

		// Token: 0x04002E5A RID: 11866
		private int[] grid;

		// Token: 0x04002E5B RID: 11867
		private int mapSizeX;

		// Token: 0x04002E5C RID: 11868
		private int mapSizeZ;
	}
}
