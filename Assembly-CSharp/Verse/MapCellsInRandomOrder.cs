using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F5E RID: 3934
	public class MapCellsInRandomOrder
	{
		// Token: 0x04003E6E RID: 15982
		private Map map;

		// Token: 0x04003E6F RID: 15983
		private List<IntVec3> randomizedCells;

		// Token: 0x06005F46 RID: 24390 RVA: 0x00309656 File Offset: 0x00307A56
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x00309668 File Offset: 0x00307A68
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x0030968C File Offset: 0x00307A8C
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x06005F49 RID: 24393 RVA: 0x003096B4 File Offset: 0x00307AB4
		private void CreateListIfShould()
		{
			if (this.randomizedCells == null)
			{
				this.randomizedCells = new List<IntVec3>(this.map.Area);
				foreach (IntVec3 item in this.map.AllCells)
				{
					this.randomizedCells.Add(item);
				}
				Rand.PushState();
				Rand.Seed = (Find.World.info.Seed ^ this.map.Tile);
				this.randomizedCells.Shuffle<IntVec3>();
				Rand.PopState();
			}
		}
	}
}
