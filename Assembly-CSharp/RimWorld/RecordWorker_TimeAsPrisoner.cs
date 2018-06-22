using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A2 RID: 1186
	public class RecordWorker_TimeAsPrisoner : RecordWorker
	{
		// Token: 0x06001546 RID: 5446 RVA: 0x000BD5E0 File Offset: 0x000BB9E0
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
