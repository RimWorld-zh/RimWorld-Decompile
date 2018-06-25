using System;

namespace Verse
{
	// Token: 0x02000B70 RID: 2928
	public abstract class RoomStatWorker
	{
		// Token: 0x04002AD8 RID: 10968
		public RoomStatDef def;

		// Token: 0x06003FED RID: 16365
		public abstract float GetScore(Room room);
	}
}
