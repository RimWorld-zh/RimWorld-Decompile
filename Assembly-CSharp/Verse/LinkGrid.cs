using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE5 RID: 3557
	public class LinkGrid
	{
		// Token: 0x040034D4 RID: 13524
		private Map map;

		// Token: 0x040034D5 RID: 13525
		private LinkFlags[] linkGrid;

		// Token: 0x06004FB0 RID: 20400 RVA: 0x002972A8 File Offset: 0x002956A8
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x002972D0 File Offset: 0x002956D0
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x00297300 File Offset: 0x00295700
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect.CellRectIterator iterator = linker.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				IntVec3 c = iterator.Current;
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
				iterator.MoveNext();
			}
		}
	}
}
