using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004A7 RID: 1191
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x0600154F RID: 5455 RVA: 0x000BD99C File Offset: 0x000BBD9C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
