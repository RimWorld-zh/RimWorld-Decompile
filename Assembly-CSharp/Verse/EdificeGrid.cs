using System;

namespace Verse
{
	// Token: 0x02000C20 RID: 3104
	public sealed class EdificeGrid
	{
		// Token: 0x04002E5D RID: 11869
		private Map map;

		// Token: 0x04002E5E RID: 11870
		private Building[] innerArray;

		// Token: 0x060043F5 RID: 17397 RVA: 0x0023D793 File Offset: 0x0023BB93
		public EdificeGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Building[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0023D7BC File Offset: 0x0023BBBC
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

		// Token: 0x060043F9 RID: 17401 RVA: 0x0023D828 File Offset: 0x0023BC28
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

		// Token: 0x060043FA RID: 17402 RVA: 0x0023D934 File Offset: 0x0023BD34
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
	}
}
