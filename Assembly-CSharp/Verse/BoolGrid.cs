using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C1D RID: 3101
	public class BoolGrid : IExposable
	{
		// Token: 0x060043AC RID: 17324 RVA: 0x0023B3CC File Offset: 0x002397CC
		public BoolGrid()
		{
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0023B3DC File Offset: 0x002397DC
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060043AE RID: 17326 RVA: 0x0023B3F4 File Offset: 0x002397F4
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060043AF RID: 17327 RVA: 0x0023B410 File Offset: 0x00239810
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

		// Token: 0x060043B6 RID: 17334 RVA: 0x0023B4D8 File Offset: 0x002398D8
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0023B520 File Offset: 0x00239920
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

		// Token: 0x060043B8 RID: 17336 RVA: 0x0023B594 File Offset: 0x00239994
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0023B5F5 File Offset: 0x002399F5
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0023B613 File Offset: 0x00239A13
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x0023B62C File Offset: 0x00239A2C
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

		// Token: 0x060043BC RID: 17340 RVA: 0x0023B680 File Offset: 0x00239A80
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
		}

		// Token: 0x04002E45 RID: 11845
		private bool[] arr;

		// Token: 0x04002E46 RID: 11846
		private int trueCountInt = 0;

		// Token: 0x04002E47 RID: 11847
		private int mapSizeX;

		// Token: 0x04002E48 RID: 11848
		private int mapSizeZ;
	}
}
