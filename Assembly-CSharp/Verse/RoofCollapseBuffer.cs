using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C9A RID: 3226
	public class RoofCollapseBuffer
	{
		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600471A RID: 18202 RVA: 0x002584E8 File Offset: 0x002568E8
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x00258504 File Offset: 0x00256904
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x00258525 File Offset: 0x00256925
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x00258545 File Offset: 0x00256945
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x04003058 RID: 12376
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
