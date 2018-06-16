using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C9E RID: 3230
	public class RoofCollapseBuffer
	{
		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06004713 RID: 18195 RVA: 0x00257120 File Offset: 0x00255520
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0025713C File Offset: 0x0025553C
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0025715D File Offset: 0x0025555D
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0025717D File Offset: 0x0025557D
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x0400304F RID: 12367
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
