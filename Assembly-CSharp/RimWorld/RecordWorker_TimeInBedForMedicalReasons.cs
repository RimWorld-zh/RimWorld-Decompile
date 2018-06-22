using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A8 RID: 1192
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x06001552 RID: 5458 RVA: 0x000BD6EC File Offset: 0x000BBAEC
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
