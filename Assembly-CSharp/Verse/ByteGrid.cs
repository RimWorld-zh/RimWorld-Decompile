using System;

namespace Verse
{
	// Token: 0x02000C1D RID: 3101
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x060043BB RID: 17339 RVA: 0x0023B854 File Offset: 0x00239C54
		public ByteGrid()
		{
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x0023B85D File Offset: 0x00239C5D
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A9B RID: 2715
		public byte this[IntVec3 c]
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

		// Token: 0x17000A9C RID: 2716
		public byte this[int index]
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

		// Token: 0x17000A9D RID: 2717
		public byte this[int x, int z]
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

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x060043C3 RID: 17347 RVA: 0x0023B92C File Offset: 0x00239D2C
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x0023B94C File Offset: 0x00239D4C
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0023B994 File Offset: 0x00239D94
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
				this.grid = new byte[this.mapSizeX * this.mapSizeZ];
			}
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x0023BA06 File Offset: 0x00239E06
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x0023BA40 File Offset: 0x00239E40
		public void Clear(byte value = 0)
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

		// Token: 0x060043C8 RID: 17352 RVA: 0x0023BA94 File Offset: 0x00239E94
		public void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					IntVec3 c = CellIndicesUtility.IndexToCell(i, this.mapSizeX);
					CellRenderer.RenderCell(c, (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x04002E47 RID: 11847
		private byte[] grid;

		// Token: 0x04002E48 RID: 11848
		private int mapSizeX;

		// Token: 0x04002E49 RID: 11849
		private int mapSizeZ;
	}
}
