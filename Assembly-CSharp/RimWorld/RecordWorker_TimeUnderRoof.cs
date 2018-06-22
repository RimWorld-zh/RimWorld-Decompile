using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AB RID: 1195
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x06001558 RID: 5464 RVA: 0x000BD7A8 File Offset: 0x000BBBA8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
