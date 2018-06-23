using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B69 RID: 2921
	public class RoomRoleDef : Def
	{
		// Token: 0x04002AC2 RID: 10946
		public Type workerClass;

		// Token: 0x04002AC3 RID: 10947
		private List<RoomStatDef> relatedStats = null;

		// Token: 0x04002AC4 RID: 10948
		[Unsaved]
		private RoomRoleWorker workerInt = null;

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003FDE RID: 16350 RVA: 0x0021AD6C File Offset: 0x0021916C
		public RoomRoleWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0021ADA8 File Offset: 0x002191A8
		public bool IsStatRelated(RoomStatDef def)
		{
			bool result;
			if (this.relatedStats == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.relatedStats.Count; i++)
				{
					if (this.relatedStats[i] == def)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
