using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A4 RID: 1188
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x06001549 RID: 5449 RVA: 0x000BD930 File Offset: 0x000BBD30
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
