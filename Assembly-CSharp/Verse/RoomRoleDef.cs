using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000B6C RID: 2924
	public class RoomRoleDef : Def
	{
		// Token: 0x04002AC9 RID: 10953
		public Type workerClass;

		// Token: 0x04002ACA RID: 10954
		private List<RoomStatDef> relatedStats = null;

		// Token: 0x04002ACB RID: 10955
		[Unsaved]
		private RoomRoleWorker workerInt = null;

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003FE1 RID: 16353 RVA: 0x0021B128 File Offset: 0x00219528
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

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0021B164 File Offset: 0x00219564
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
