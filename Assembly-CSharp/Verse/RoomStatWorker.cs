using System;

namespace Verse
{
	// Token: 0x02000B6D RID: 2925
	public abstract class RoomStatWorker
	{
		// Token: 0x04002AD1 RID: 10961
		public RoomStatDef def;

		// Token: 0x06003FEA RID: 16362
		public abstract float GetScore(Room room);
	}
}
