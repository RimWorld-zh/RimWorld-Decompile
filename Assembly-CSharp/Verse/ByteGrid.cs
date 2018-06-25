using System;

namespace Verse
{
	// Token: 0x02000C1D RID: 3101
	public sealed class ByteGrid : IExposable
	{
		// Token: 0x04002E58 RID: 11864
		private byte[] grid;

		// Token: 0x04002E59 RID: 11865
		private int mapSizeX;

		// Token: 0x04002E5A RID: 11866
		private int mapSizeZ;

		// Token: 0x060043C7 RID: 17351 RVA: 0x0023CFD8 File Offset: 0x0023B3D8
		public ByteGrid()
		{
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x0023CFE1 File Offset: 0x0023B3E1
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
		// (get) Token: 0x060043CF RID: 17359 RVA: 0x0023D0B0 File Offset: 0x0023B4B0
		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0023D0D0 File Offset: 0x0023B4D0
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0023D118 File Offset: 0x0023B518
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

		// Token: 0x060043D2 RID: 17362 RVA: 0x0023D18A File Offset: 0x0023B58A
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.ByteArray(ref this.grid, "grid");
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0023D1C4 File Offset: 0x0023B5C4
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

		// Token: 0x060043D4 RID: 17364 RVA: 0x0023D218 File Offset: 0x0023B618
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
	}
}
