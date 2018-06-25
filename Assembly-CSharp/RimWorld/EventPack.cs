using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CD RID: 2253
	public struct EventPack
	{
		// Token: 0x04001BAB RID: 7083
		private string tagInt;

		// Token: 0x04001BAC RID: 7084
		private IntVec3 cellInt;

		// Token: 0x04001BAD RID: 7085
		private IEnumerable<IntVec3> cellsInt;

		// Token: 0x0600338F RID: 13199 RVA: 0x001B9CFD File Offset: 0x001B80FD
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x001B9D19 File Offset: 0x001B8119
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x001B9D31 File Offset: 0x001B8131
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003392 RID: 13202 RVA: 0x001B9D50 File Offset: 0x001B8150
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x001B9D6C File Offset: 0x001B816C
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x001B9D88 File Offset: 0x001B8188
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x001B9DA4 File Offset: 0x001B81A4
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x001B9DC0 File Offset: 0x001B81C0
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
	}
}
