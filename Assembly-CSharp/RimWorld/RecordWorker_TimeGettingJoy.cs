using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004A9 RID: 1193
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x06001555 RID: 5461 RVA: 0x000BD650 File Offset: 0x000BBA50
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
