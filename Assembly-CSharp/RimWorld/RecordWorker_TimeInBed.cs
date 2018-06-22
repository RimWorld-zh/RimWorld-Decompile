using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A7 RID: 1191
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x000BD6C8 File Offset: 0x000BBAC8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
