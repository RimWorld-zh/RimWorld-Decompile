using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AB RID: 1195
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06001559 RID: 5465 RVA: 0x000BD6CC File Offset: 0x000BBACC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
