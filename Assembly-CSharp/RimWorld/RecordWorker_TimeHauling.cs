using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A8 RID: 1192
	public class RecordWorker_TimeHauling : RecordWorker
	{
		// Token: 0x06001551 RID: 5457 RVA: 0x000BD9DC File Offset: 0x000BBDDC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return !pawn.Dead && pawn.carryTracker.CarriedThing != null;
		}
	}
}
