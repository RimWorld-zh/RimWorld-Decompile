using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C19 RID: 3097
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

		// Token: 0x060043B3 RID: 17331 RVA: 0x0023C76C File Offset: 0x0023AB6C
		public BoolGrid()
		{
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0023C77C File Offset: 0x0023AB7C
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060043B5 RID: 17333 RVA: 0x0023C794 File Offset: 0x0023AB94
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x060043B6 RID: 17334 RVA: 0x0023C7B0 File Offset: 0x0023ABB0
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

		// Token: 0x17000A9A RID: 2714
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

		// Token: 0x17000A9B RID: 2715
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

		// Token: 0x17000A9C RID: 2716
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

		// Token: 0x060043BD RID: 17341 RVA: 0x0023C878 File Offset: 0x0023AC78
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0023C8C0 File Offset: 0x0023ACC0
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

		// Token: 0x060043BF RID: 17343 RVA: 0x0023C934 File Offset: 0x0023AD34
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x0023C995 File Offset: 0x0023AD95
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0023C9B3 File Offset: 0x0023ADB3
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x0023C9CC File Offset: 0x0023ADCC
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

		// Token: 0x060043C3 RID: 17347 RVA: 0x0023CA20 File Offset: 0x0023AE20
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
