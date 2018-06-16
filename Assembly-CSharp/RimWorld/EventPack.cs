using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CF RID: 2255
	public struct EventPack
	{
		// Token: 0x06003390 RID: 13200 RVA: 0x001B9639 File Offset: 0x001B7A39
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x001B9655 File Offset: 0x001B7A55
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x001B966D File Offset: 0x001B7A6D
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x001B968C File Offset: 0x001B7A8C
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x001B96A8 File Offset: 0x001B7AA8
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x001B96C4 File Offset: 0x001B7AC4
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x001B96E0 File Offset: 0x001B7AE0
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x001B96FC File Offset: 0x001B7AFC
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

		// Token: 0x04001BA7 RID: 7079
		private string tagInt;

		// Token: 0x04001BA8 RID: 7080
		private IntVec3 cellInt;

		// Token: 0x04001BA9 RID: 7081
		private IEnumerable<IntVec3> cellsInt;
	}
}
