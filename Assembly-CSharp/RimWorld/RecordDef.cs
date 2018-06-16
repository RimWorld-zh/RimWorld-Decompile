using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BE RID: 702
	public class RecordDef : Def
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x00069318 File Offset: 0x00067718
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

		// Token: 0x040006DD RID: 1757
		public RecordType type;

		// Token: 0x040006DE RID: 1758
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x040006DF RID: 1759
		public List<JobDef> measuredTimeJobs;

		// Token: 0x040006E0 RID: 1760
		[Unsaved]
		private RecordWorker workerInt;
	}
}
