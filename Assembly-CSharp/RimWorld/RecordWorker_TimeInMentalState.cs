using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AB RID: 1195
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x000BDAB0 File Offset: 0x000BBEB0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
