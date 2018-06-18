using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C1B RID: 3099
	public sealed class BlueprintGrid
	{
		// Token: 0x060043A6 RID: 17318 RVA: 0x0023B230 File Offset: 0x00239630
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060043A7 RID: 17319 RVA: 0x0023B258 File Offset: 0x00239658
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0023B274 File Offset: 0x00239674
		public void Register(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					if (this.innerArray[num] == null)
					{
						this.innerArray[num] = new List<Blueprint>();
					}
					this.innerArray[num].Add(ed);
				}
			}
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0023B30C File Offset: 0x0023970C
		public void DeRegister(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					this.innerArray[num].Remove(ed);
					if (this.innerArray[num].Count == 0)
					{
						this.innerArray[num] = null;
					}
				}
			}
		}

		// Token: 0x04002E41 RID: 11841
		private Map map;

		// Token: 0x04002E42 RID: 11842
		private List<Blueprint>[] innerArray;
	}
}
