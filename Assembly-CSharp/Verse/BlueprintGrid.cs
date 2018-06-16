using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C1C RID: 3100
	public sealed class BlueprintGrid
	{
		// Token: 0x060043A8 RID: 17320 RVA: 0x0023B258 File Offset: 0x00239658
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060043A9 RID: 17321 RVA: 0x0023B280 File Offset: 0x00239680
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0023B29C File Offset: 0x0023969C
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

		// Token: 0x060043AB RID: 17323 RVA: 0x0023B334 File Offset: 0x00239734
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

		// Token: 0x04002E43 RID: 11843
		private Map map;

		// Token: 0x04002E44 RID: 11844
		private List<Blueprint>[] innerArray;
	}
}
