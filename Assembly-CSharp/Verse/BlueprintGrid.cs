using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C1A RID: 3098
	public sealed class BlueprintGrid
	{
		// Token: 0x04002E4B RID: 11851
		private Map map;

		// Token: 0x04002E4C RID: 11852
		private List<Blueprint>[] innerArray;

		// Token: 0x060043B2 RID: 17330 RVA: 0x0023C6D4 File Offset: 0x0023AAD4
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x0023C6FC File Offset: 0x0023AAFC
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0023C718 File Offset: 0x0023AB18
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

		// Token: 0x060043B5 RID: 17333 RVA: 0x0023C7B0 File Offset: 0x0023ABB0
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
