using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AB RID: 1195
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x06001558 RID: 5464 RVA: 0x000BD8B0 File Offset: 0x000BBCB0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
