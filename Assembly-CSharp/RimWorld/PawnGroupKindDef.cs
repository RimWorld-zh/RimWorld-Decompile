using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B4 RID: 692
	public class PawnGroupKindDef : Def
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x00068A38 File Offset: 0x00066E38
		public PawnGroupKindWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnGroupKindWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040006AF RID: 1711
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x040006B0 RID: 1712
		[Unsaved]
		private PawnGroupKindWorker workerInt;
	}
}
