using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AF RID: 1199
	public class RecordWorker_TimeUnderRoof : RecordWorker
	{
		// Token: 0x06001561 RID: 5473 RVA: 0x000BD790 File Offset: 0x000BBB90
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Position.Roofed(pawn.Map);
		}
	}
}
