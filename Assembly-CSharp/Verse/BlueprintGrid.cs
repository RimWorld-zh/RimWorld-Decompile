using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C1B RID: 3099
	public sealed class BlueprintGrid
	{
		// Token: 0x04002E52 RID: 11858
		private Map map;

		// Token: 0x04002E53 RID: 11859
		private List<Blueprint>[] innerArray;

		// Token: 0x060043B2 RID: 17330 RVA: 0x0023C9B4 File Offset: 0x0023ADB4
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x0023C9DC File Offset: 0x0023ADDC
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0023C9F8 File Offset: 0x0023ADF8
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

		// Token: 0x060043B5 RID: 17333 RVA: 0x0023CA90 File Offset: 0x0023AE90
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
	}
}
