using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A6 RID: 1190
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x000BD978 File Offset: 0x000BBD78
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
