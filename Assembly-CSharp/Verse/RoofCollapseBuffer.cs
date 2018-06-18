using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C9D RID: 3229
	public class RoofCollapseBuffer
	{
		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06004711 RID: 18193 RVA: 0x002570F8 File Offset: 0x002554F8
		public List<IntVec3> CellsMarkedToCollapse
		{
			get
			{
				return this.cellsToCollapse;
			}
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00257114 File Offset: 0x00255514
		public bool IsMarkedToCollapse(IntVec3 c)
		{
			return this.cellsToCollapse.Contains(c);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00257135 File Offset: 0x00255535
		public void MarkToCollapse(IntVec3 c)
		{
			if (!this.cellsToCollapse.Contains(c))
			{
				this.cellsToCollapse.Add(c);
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x00257155 File Offset: 0x00255555
		public void Clear()
		{
			this.cellsToCollapse.Clear();
		}

		// Token: 0x0400304D RID: 12365
		private List<IntVec3> cellsToCollapse = new List<IntVec3>();
	}
}
