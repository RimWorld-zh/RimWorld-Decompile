using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AA RID: 1194
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x06001555 RID: 5461 RVA: 0x000BDA3C File Offset: 0x000BBE3C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
