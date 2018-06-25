using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C9D RID: 3229
	public class RoofCollapseBuffer
	{
		// Token: 0x0400305F RID: 12383
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x0600471D RID: 18205 RVA: 0x002588A4 File Offset: 0x00256CA4
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x002588C0 File Offset: 0x00256CC0
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x002588E1 File Offset: 0x00256CE1
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x00258901 File Offset: 0x00256D01
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}
	}
}
