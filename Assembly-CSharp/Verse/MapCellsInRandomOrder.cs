using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F5E RID: 3934
	public class MapCellsInRandomOrder
	{
		// Token: 0x06005F1D RID: 24349 RVA: 0x003075B2 File Offset: 0x003059B2
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005F1E RID: 24350 RVA: 0x003075C4 File Offset: 0x003059C4
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06005F1F RID: 24351 RVA: 0x003075E8 File Offset: 0x003059E8
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x00307610 File Offset: 0x00305A10
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

		// Token: 0x04003E5C RID: 15964
		private Map map;

		// Token: 0x04003E5D RID: 15965
		private List<IntVec3> randomizedCells;
	}
}
