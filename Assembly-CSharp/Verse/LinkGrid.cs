using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE2 RID: 3554
	public class LinkGrid
	{
		// Token: 0x040034CD RID: 13517
		private Map map;

		// Token: 0x040034CE RID: 13518
		private LinkFlags[] linkGrid;

		// Token: 0x06004FAC RID: 20396 RVA: 0x00296E9C File Offset: 0x0029529C
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x00296EC4 File Offset: 0x002952C4
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00296EF4 File Offset: 0x002952F4
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
