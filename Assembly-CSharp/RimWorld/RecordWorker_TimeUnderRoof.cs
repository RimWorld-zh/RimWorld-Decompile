using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AD RID: 1197
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x0600155B RID: 5467 RVA: 0x000BDAF8 File Offset: 0x000BBEF8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
