using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A9 RID: 1193
	public class RecordWorker_TimeInMentalState : RecordWorker
	{
		// Token: 0x06001554 RID: 5460 RVA: 0x000BD760 File Offset: 0x000BBB60
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InMentalState;
		}
	}
}
