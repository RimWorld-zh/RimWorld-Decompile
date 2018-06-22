using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004A5 RID: 1189
	public class RecordWorker_TimeGettingJoy : RecordWorker
	{
		// Token: 0x0600154C RID: 5452 RVA: 0x000BD64C File Offset: 0x000BBA4C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return curJob != null && curJob.def.joyKind != null;
		}
	}
}
