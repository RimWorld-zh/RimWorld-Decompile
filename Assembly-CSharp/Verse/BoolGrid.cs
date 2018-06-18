using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C1C RID: 3100
	public class BoolGrid : IExposable
	{
		// Token: 0x060043AA RID: 17322 RVA: 0x0023B3A4 File Offset: 0x002397A4
		public BoolGrid()
		{
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0023B3B4 File Offset: 0x002397B4
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060043AC RID: 17324 RVA: 0x0023B3CC File Offset: 0x002397CC
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060043AD RID: 17325 RVA: 0x0023B3E8 File Offset: 0x002397E8
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

		// Token: 0x17000A98 RID: 2712
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

		// Token: 0x17000A99 RID: 2713
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

		// Token: 0x17000A9A RID: 2714
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

		// Token: 0x060043B4 RID: 17332 RVA: 0x0023B4B0 File Offset: 0x002398B0
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0023B4F8 File Offset: 0x002398F8
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

		// Token: 0x060043B6 RID: 17334 RVA: 0x0023B56C File Offset: 0x0023996C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0023B5CD File Offset: 0x002399CD
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0023B5EB File Offset: 0x002399EB
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0023B604 File Offset: 0x00239A04
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

		// Token: 0x060043BA RID: 17338 RVA: 0x0023B658 File Offset: 0x00239A58
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
		}

		// Token: 0x04002E43 RID: 11843
		private bool[] arr;

		// Token: 0x04002E44 RID: 11844
		private int trueCountInt = 0;

		// Token: 0x04002E45 RID: 11845
		private int mapSizeX;

		// Token: 0x04002E46 RID: 11846
		private int mapSizeZ;
	}
}
