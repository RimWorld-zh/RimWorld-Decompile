using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B6 RID: 694
	public class PawnGroupKindDef : Def
	{
		// Token: 0x040006B0 RID: 1712
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x040006B1 RID: 1713
		[Unsaved]
		private PawnGroupKindWorker workerInt;

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00068BEC File Offset: 0x00066FEC
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
	}
}
