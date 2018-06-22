using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A6 RID: 1190
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x0600154E RID: 5454 RVA: 0x000BD68C File Offset: 0x000BBA8C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
