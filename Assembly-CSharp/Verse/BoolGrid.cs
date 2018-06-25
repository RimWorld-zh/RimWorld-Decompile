using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C1C RID: 3100
	public class BoolGrid : IExposable
	{
		// Token: 0x04002E54 RID: 11860
		private bool[] arr;

		// Token: 0x04002E55 RID: 11861
		private int trueCountInt = 0;

		// Token: 0x04002E56 RID: 11862
		private int mapSizeX;

		// Token: 0x04002E57 RID: 11863
		private int mapSizeZ;

		// Token: 0x060043B6 RID: 17334 RVA: 0x0023CB28 File Offset: 0x0023AF28
		public BoolGrid()
		{
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0023CB38 File Offset: 0x0023AF38
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060043B8 RID: 17336 RVA: 0x0023CB50 File Offset: 0x0023AF50
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x0023CB6C File Offset: 0x0023AF6C
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

		// Token: 0x060043C0 RID: 17344 RVA: 0x0023CC34 File Offset: 0x0023B034
		public bool MapSizeMatches(Map map)
		{
			return this.mapSizeX == map.Size.x && this.mapSizeZ == map.Size.z;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0023CC7C File Offset: 0x0023B07C
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

		// Token: 0x060043C2 RID: 17346 RVA: 0x0023CCF0 File Offset: 0x0023B0F0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeX, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.mapSizeZ, "mapSizeZ", 0, false);
			DataExposeUtility.BoolArray(ref this.arr, this.mapSizeX * this.mapSizeZ, "arr");
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0023CD51 File Offset: 0x0023B151
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x0023CD6F File Offset: 0x0023B16F
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.mapSizeX), value);
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0023CD88 File Offset: 0x0023B188
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

		// Token: 0x060043C6 RID: 17350 RVA: 0x0023CDDC File Offset: 0x0023B1DC
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
