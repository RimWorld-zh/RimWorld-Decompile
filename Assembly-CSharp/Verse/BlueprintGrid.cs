using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C18 RID: 3096
	public sealed class BlueprintGrid
	{
		// Token: 0x04002E4B RID: 11851
		private Map map;

		// Token: 0x04002E4C RID: 11852
		private List<Blueprint>[] innerArray;

		// Token: 0x060043AF RID: 17327 RVA: 0x0023C5F8 File Offset: 0x0023A9F8
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x0023C620 File Offset: 0x0023AA20
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x0023C63C File Offset: 0x0023AA3C
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

		// Token: 0x060043B2 RID: 17330 RVA: 0x0023C6D4 File Offset: 0x0023AAD4
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
