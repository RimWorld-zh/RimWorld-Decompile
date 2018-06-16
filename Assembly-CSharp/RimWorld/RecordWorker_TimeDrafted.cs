using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A8 RID: 1192
	public class RecordWorker_TimeDrafted : RecordWorker
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x000BD610 File Offset: 0x000BBA10
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
