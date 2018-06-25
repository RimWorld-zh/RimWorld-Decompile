using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004A7 RID: 1191
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x000BD79C File Offset: 0x000BBB9C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
