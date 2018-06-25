using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AA RID: 1194
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x06001556 RID: 5462 RVA: 0x000BD83C File Offset: 0x000BBC3C
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
