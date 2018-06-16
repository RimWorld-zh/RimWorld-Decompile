using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A6 RID: 1190
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x0600154F RID: 5455 RVA: 0x000BD5C8 File Offset: 0x000BB9C8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
