using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A9 RID: 1193
	public class RecordWorker_TimeInBed : RecordWorker
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x000BD818 File Offset: 0x000BBC18
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
