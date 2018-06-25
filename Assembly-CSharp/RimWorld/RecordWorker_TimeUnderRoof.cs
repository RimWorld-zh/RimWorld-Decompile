using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AD RID: 1197
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x0600155C RID: 5468 RVA: 0x000BD8F8 File Offset: 0x000BBCF8
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
