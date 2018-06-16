using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AA RID: 1194
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x000BD674 File Offset: 0x000BBA74
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
