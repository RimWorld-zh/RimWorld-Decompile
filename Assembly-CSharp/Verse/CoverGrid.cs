using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C1F RID: 3103
	public sealed class CoverGrid
	{
		// Token: 0x04002E5E RID: 11870
		private Map map;

		// Token: 0x04002E5F RID: 11871
		private Thing[] innerArray;

		// Token: 0x060043E1 RID: 17377 RVA: 0x0023D4C7 File Offset: 0x0023B8C7
		public CoverGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Thing[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AA4 RID: 2724
		public Thing this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x17000AA5 RID: 2725
		public Thing this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0023D540 File Offset: 0x0023B940
		public void Register(Thing t)
		{
			if (t.def.Fillage != FillCategory.None)
			{
				CellRect cellRect = t.OccupiedRect();
				for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
				{
					for (int j = cellRect.minX; j <= cellRect.maxX; j++)
					{
						IntVec3 c = new IntVec3(j, 0, i);
						this.RecalculateCell(c, null);
					}
				}
			}
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0023D5BC File Offset: 0x0023B9BC
		public void DeRegister(Thing t)
		{
			if (t.def.Fillage != FillCategory.None)
			{
				CellRect cellRect = t.OccupiedRect();
				for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
				{
					for (int j = cellRect.minX; j <= cellRect.maxX; j++)
					{
						IntVec3 c = new IntVec3(j, 0, i);
						this.RecalculateCell(c, t);
					}
				}
			}
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0023D638 File Offset: 0x0023BA38
		private void RecalculateCell(IntVec3 c, Thing ignoreThing = null)
		{
			Thing thing = null;
			float num = 0.001f;
			List<Thing> list = this.map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2 != ignoreThing && !thing2.Destroyed && thing2.Spawned)
				{
					if (thing2.def.fillPercent > num)
					{
						thing = thing2;
						num = thing2.def.fillPercent;
					}
				}
			}
			this.innerArray[this.map.cellIndices.CellToIndex(c)] = thing;
		}
	}
}
