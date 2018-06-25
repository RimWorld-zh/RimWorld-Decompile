using System;

namespace Verse
{
	// Token: 0x02000B6F RID: 2927
	public abstract class RoomStatWorker
	{
		// Token: 0x04002AD1 RID: 10961
		public RoomStatDef def;

		// Token: 0x06003FED RID: 16365
		public abstract float GetScore(Room room);
	}
}
