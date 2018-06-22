using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CB RID: 2251
	public struct EventPack
	{
		// Token: 0x0600338B RID: 13195 RVA: 0x001B98E9 File Offset: 0x001B7CE9
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x001B9905 File Offset: 0x001B7D05
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x001B991D File Offset: 0x001B7D1D
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x001B993C File Offset: 0x001B7D3C
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x001B9958 File Offset: 0x001B7D58
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x001B9974 File Offset: 0x001B7D74
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x001B9990 File Offset: 0x001B7D90
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x001B99AC File Offset: 0x001B7DAC
		public override string ToString()
		{
			string result;
			if (this.Cell.IsValid)
			{
				result = this.Tag + "-" + this.Cell;
			}
			else
			{
				result = this.Tag;
			}
			return result;
		}

		// Token: 0x04001BA5 RID: 7077
		private string tagInt;

		// Token: 0x04001BA6 RID: 7078
		private IntVec3 cellInt;

		// Token: 0x04001BA7 RID: 7079
		private IEnumerable<IntVec3> cellsInt;
	}
}
