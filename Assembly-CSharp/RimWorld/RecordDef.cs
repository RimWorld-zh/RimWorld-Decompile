using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BE RID: 702
	public class RecordDef : Def
	{
		// Token: 0x040006DC RID: 1756
		public RecordType type;

		// Token: 0x040006DD RID: 1757
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x040006DE RID: 1758
		public List<JobDef> measuredTimeJobs;

		// Token: 0x040006DF RID: 1759
		[Unsaved]
		private RecordWorker workerInt;

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00069380 File Offset: 0x00067780
		public RecordWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecordWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}
	}
}
