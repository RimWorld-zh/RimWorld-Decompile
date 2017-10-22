namespace Verse
{
	public class CellGrid
	{
		private int[] grid;

		private int mapSizeX;

		private int mapSizeZ;

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

		public int CellsCount
		{
			get
			{
				return this.grid.Length;
			}
		}

		public CellGrid()
		{
		}

		public CellGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		public bool MapSizeMatches(Map map)
		{
			int num = this.mapSizeX;
			IntVec3 size = map.Size;
			int result;
			if (num == size.x)
			{
				int num2 = this.mapSizeZ;
				IntVec3 size2 = map.Size;
				result = ((num2 == size2.z) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.grid != null)
			{
				this.Clear();
			}
			else
			{
				IntVec3 size = map.Size;
				this.mapSizeX = size.x;
				IntVec3 size2 = map.Size;
				this.mapSizeZ = size2.z;
				this.grid = new int[this.mapSizeX * this.mapSizeZ];
				this.Clear();
			}
		}

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
