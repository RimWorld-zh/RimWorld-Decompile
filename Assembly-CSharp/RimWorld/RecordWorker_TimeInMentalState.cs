using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AD RID: 1197
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x0600155D RID: 5469 RVA: 0x000BD748 File Offset: 0x000BBB48
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
