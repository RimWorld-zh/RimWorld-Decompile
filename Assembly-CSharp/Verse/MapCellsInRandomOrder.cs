using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F62 RID: 3938
	public class MapCellsInRandomOrder
	{
		// Token: 0x04003E71 RID: 15985
		private Map map;

		// Token: 0x04003E72 RID: 15986
		private List<IntVec3> randomizedCells;

		// Token: 0x06005F50 RID: 24400 RVA: 0x00309CD6 File Offset: 0x003080D6
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x00309CE8 File Offset: 0x003080E8
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x00309D0C File Offset: 0x0030810C
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x00309D34 File Offset: 0x00308134
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
