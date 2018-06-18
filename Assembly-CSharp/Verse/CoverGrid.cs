using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C1F RID: 3103
	public sealed class CoverGrid
	{
		// Token: 0x060043D5 RID: 17365 RVA: 0x0023BD43 File Offset: 0x0023A143
		public CoverGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Thing[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AA3 RID: 2723
		public Thing this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x17000AA4 RID: 2724
		public Thing this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0023BDBC File Offset: 0x0023A1BC
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

		// Token: 0x060043D9 RID: 17369 RVA: 0x0023BE38 File Offset: 0x0023A238
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

		// Token: 0x060043DA RID: 17370 RVA: 0x0023BEB4 File Offset: 0x0023A2B4
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

		// Token: 0x04002E4D RID: 11853
		private Map map;

		// Token: 0x04002E4E RID: 11854
		private Thing[] innerArray;
	}
}
