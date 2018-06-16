using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE6 RID: 3558
	public class LinkGrid
	{
		// Token: 0x06004F99 RID: 20377 RVA: 0x002958E0 File Offset: 0x00293CE0
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00295908 File Offset: 0x00293D08
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x00295938 File Offset: 0x00293D38
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

		// Token: 0x040034C4 RID: 13508
		private Map map;

		// Token: 0x040034C5 RID: 13509
		private LinkFlags[] linkGrid;
	}
}
