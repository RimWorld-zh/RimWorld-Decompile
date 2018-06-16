using System;

namespace Verse
{
	// Token: 0x02000C1E RID: 3102
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x060043BD RID: 17341 RVA: 0x0023B87C File Offset: 0x00239C7C
		public ByteGrid()
		{
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0023B885 File Offset: 0x00239C85
		public ByteGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A9C RID: 2716
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

		// Token: 0x17000A9D RID: 2717
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

		// Token: 0x17000A9E RID: 2718
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

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x0023B954 File Offset: 0x00239D54
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x0023B974 File Offset: 0x00239D74
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x0023B9BC File Offset: 0x00239DBC
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

		// Token: 0x060043C8 RID: 17352 RVA: 0x0023BA2E File Offset: 0x00239E2E
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x0023BA68 File Offset: 0x00239E68
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

		// Token: 0x060043CA RID: 17354 RVA: 0x0023BABC File Offset: 0x00239EBC
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

		// Token: 0x04002E49 RID: 11849
		private byte[] grid;

		// Token: 0x04002E4A RID: 11850
		private int mapSizeX;

		// Token: 0x04002E4B RID: 11851
		private int mapSizeZ;
	}
}
