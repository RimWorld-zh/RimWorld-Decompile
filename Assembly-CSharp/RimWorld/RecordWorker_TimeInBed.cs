using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AB RID: 1195
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06001559 RID: 5465 RVA: 0x000BD6B0 File Offset: 0x000BBAB0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
