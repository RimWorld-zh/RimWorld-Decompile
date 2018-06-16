using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F5F RID: 3935
	public class MapCellsInRandomOrder
	{
		// Token: 0x06005F1F RID: 24351 RVA: 0x003074D6 File Offset: 0x003058D6
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x003074E8 File Offset: 0x003058E8
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06005F21 RID: 24353 RVA: 0x0030750C File Offset: 0x0030590C
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x06005F22 RID: 24354 RVA: 0x00307534 File Offset: 0x00305934
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

		// Token: 0x04003E5D RID: 15965
		private Map map;

		// Token: 0x04003E5E RID: 15966
		private List<IntVec3> randomizedCells;
	}
}
