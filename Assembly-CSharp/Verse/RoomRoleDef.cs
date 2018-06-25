using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B6B RID: 2923
	public class RoomRoleDef : Def
	{
		// Token: 0x04002AC2 RID: 10946
		public Type workerClass;

		// Token: 0x04002AC3 RID: 10947
		private List<RoomStatDef> relatedStats = null;

		// Token: 0x04002AC4 RID: 10948
		[Unsaved]
		private RoomRoleWorker workerInt = null;

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x0021AE48 File Offset: 0x00219248
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

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0021AE84 File Offset: 0x00219284
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
