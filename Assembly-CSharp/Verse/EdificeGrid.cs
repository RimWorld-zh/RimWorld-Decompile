using System;

namespace Verse
{
	// Token: 0x02000C22 RID: 3106
	public sealed class EdificeGrid
	{
		// Token: 0x060043EB RID: 17387 RVA: 0x0023C317 File Offset: 0x0023A717
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060043EC RID: 17388 RVA: 0x0023C340 File Offset: 0x0023A740
		public Building[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		public Building this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x17000AA9 RID: 2729
		public Building this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0023C3AC File Offset: 0x0023A7AC
		public void Register(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 intVec = new IntVec3(j, 0, i);
					if (UnityData.isDebugBuild && this[intVec] != null && !this[intVec].Destroyed)
					{
						Log.Error(string.Concat(new object[]
						{
							"Added edifice ",
							ed.LabelCap,
							" over edifice ",
							this[intVec].LabelCap,
							" at ",
							intVec,
							". Destroying old edifice."
						}), false);
						this[intVec].Destroy(DestroyMode.Vanish);
						return;
					}
					this.innerArray[cellIndices.CellToIndex(intVec)] = ed;
				}
			}
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0023C4B8 File Offset: 0x0023A8B8
		public void DeRegister(Building ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.innerArray[cellIndices.CellToIndex(j, i)] = null;
				}
			}
		}

		// Token: 0x04002E55 RID: 11861
		private Map map;

		// Token: 0x04002E56 RID: 11862
		private Building[] innerArray;
	}
}
