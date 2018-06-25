using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A9 RID: 1193
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x000BDA18 File Offset: 0x000BBE18
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
