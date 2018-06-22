using System;

namespace Verse
{
	// Token: 0x02000C1A RID: 3098
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x060043C4 RID: 17348 RVA: 0x0023CC1C File Offset: 0x0023B01C
		public ByteGrid()
		{
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0023CC25 File Offset: 0x0023B025
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A9D RID: 2717
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

		// Token: 0x17000A9E RID: 2718
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

		// Token: 0x17000A9F RID: 2719
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

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060043CC RID: 17356 RVA: 0x0023CCF4 File Offset: 0x0023B0F4
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0023CD14 File Offset: 0x0023B114
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0023CD5C File Offset: 0x0023B15C
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

		// Token: 0x060043CF RID: 17359 RVA: 0x0023CDCE File Offset: 0x0023B1CE
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0023CE08 File Offset: 0x0023B208
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

		// Token: 0x060043D1 RID: 17361 RVA: 0x0023CE5C File Offset: 0x0023B25C
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

		// Token: 0x04002E51 RID: 11857
		private byte[] grid;

		// Token: 0x04002E52 RID: 11858
		private int mapSizeX;

		// Token: 0x04002E53 RID: 11859
		private int mapSizeZ;
	}
}
