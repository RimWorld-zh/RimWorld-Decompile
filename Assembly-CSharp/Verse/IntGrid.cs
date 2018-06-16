using System;

namespace Verse
{
	// Token: 0x02000C26 RID: 3110
	public sealed class IntGrid
	{
		// Token: 0x06004420 RID: 17440 RVA: 0x0023D216 File Offset: 0x0023B616
		public IntGrid()
		{
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x0023D21F File Offset: 0x0023B61F
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000AAE RID: 2734
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

		// Token: 0x17000AAF RID: 2735
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

		// Token: 0x17000AB0 RID: 2736
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

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06004428 RID: 17448 RVA: 0x0023D2EC File Offset: 0x0023B6EC
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0023D30C File Offset: 0x0023B70C
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x0023D354 File Offset: 0x0023B754
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

		// Token: 0x0600442B RID: 17451 RVA: 0x0023D3C8 File Offset: 0x0023B7C8
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

		// Token: 0x0600442C RID: 17452 RVA: 0x0023D41C File Offset: 0x0023B81C
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

		// Token: 0x04002E5C RID: 11868
		private int[] grid;

		// Token: 0x04002E5D RID: 11869
		private int mapSizeX;

		// Token: 0x04002E5E RID: 11870
		private int mapSizeZ;
	}
}
