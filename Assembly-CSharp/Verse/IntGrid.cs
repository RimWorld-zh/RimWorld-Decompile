using System;

namespace Verse
{
	// Token: 0x02000C22 RID: 3106
	public sealed class IntGrid
	{
		// Token: 0x04002E64 RID: 11876
		private int[] grid;

		// Token: 0x04002E65 RID: 11877
		private int mapSizeX;

		// Token: 0x04002E66 RID: 11878
		private int mapSizeZ;

		// Token: 0x06004427 RID: 17447 RVA: 0x0023E5B6 File Offset: 0x0023C9B6
		public IntGrid()
		{
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0023E5BF File Offset: 0x0023C9BF
		public IntGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000AAF RID: 2735
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

		// Token: 0x17000AB0 RID: 2736
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

		// Token: 0x17000AB1 RID: 2737
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

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x0600442F RID: 17455 RVA: 0x0023E68C File Offset: 0x0023CA8C
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x0023E6AC File Offset: 0x0023CAAC
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0023E6F4 File Offset: 0x0023CAF4
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

		// Token: 0x06004432 RID: 17458 RVA: 0x0023E768 File Offset: 0x0023CB68
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

		// Token: 0x06004433 RID: 17459 RVA: 0x0023E7BC File Offset: 0x0023CBBC
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
	}
}
