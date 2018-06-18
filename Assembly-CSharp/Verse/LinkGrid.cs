using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE5 RID: 3557
	public class LinkGrid
	{
		// Token: 0x06004F97 RID: 20375 RVA: 0x002958C0 File Offset: 0x00293CC0
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x002958E8 File Offset: 0x00293CE8
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x00295918 File Offset: 0x00293D18
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

		// Token: 0x040034C2 RID: 13506
		private Map map;

		// Token: 0x040034C3 RID: 13507
		private LinkFlags[] linkGrid;
	}
}
