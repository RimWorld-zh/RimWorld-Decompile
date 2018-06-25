using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C1B RID: 3099
	public class BoolGrid : IExposable
	{
		// Token: 0x04002E4D RID: 11853
		private bool[] arr;

		// Token: 0x04002E4E RID: 11854
		private int trueCountInt = 0;

		// Token: 0x04002E4F RID: 11855
		private int mapSizeX;

		// Token: 0x04002E50 RID: 11856
		private int mapSizeZ;

		// Token: 0x060043B6 RID: 17334 RVA: 0x0023C848 File Offset: 0x0023AC48
		public BoolGrid()
		{
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0023C858 File Offset: 0x0023AC58
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060043B8 RID: 17336 RVA: 0x0023C870 File Offset: 0x0023AC70
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x0023C88C File Offset: 0x0023AC8C
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				if (this.trueCountInt == 0)
				{
					yield break;
				}
				int yieldedCount = 0;
				for (int i = 0; i < this.arr.Length; i++)
				{
					if (this.arr[i])
					{
						yield return CellIndicesUtility.IndexToCell(i, this.mapSizeX);
						yieldedCount++;
						if (yieldedCount >= this.trueCountInt)
						{
							yield break;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000A99 RID: 2713
		public bool this[int index]
		{
			get
			{
				return this.arr[index];
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x17000A9A RID: 2714
		public bool this[IntVec3 c]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(c, this.mapSizeX)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000A9B RID: 2715
		public bool this[int x, int z]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(x, z, this.mapSizeX)];
			}
			set
			{
				this.Set(CellIndicesUtility.CellToIndex(x, z, this.mapSizeX), value);
			}
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x0023C954 File Offset: 0x0023AD54
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0023C99C File Offset: 0x0023AD9C
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map) && this.arr != null)
			{
				this.Clear();
			}
			else
			{
				this.mapSizeX = map.Size.x;
				this.mapSizeZ = map.Size.z;
				this.arr = new bool[this.mapSizeX * this.mapSizeZ];
			}
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x0023CA10 File Offset: 0x0023AE10
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0023CA71 File Offset: 0x0023AE71
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x0023CA8F File Offset: 0x0023AE8F
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0023CAA8 File Offset: 0x0023AEA8
		public virtual void Set(int index, bool value)
		{
			if (this.arr[index] != value)
			{
				this.arr[index] = value;
				if (value)
				{
					this.trueCountInt++;
				}
				else
				{
					this.trueCountInt--;
				}
			}
		}

		// Token: 0x060043C6 RID: 17350 RVA: 0x0023CAFC File Offset: 0x0023AEFC
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
		}
	}
}
