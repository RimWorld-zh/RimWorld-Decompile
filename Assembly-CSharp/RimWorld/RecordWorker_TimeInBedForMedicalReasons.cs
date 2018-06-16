using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004AC RID: 1196
	public class RecordWorker_TimeInBedForMedicalReasons : RecordWorker
	{
		// Token: 0x0600155B RID: 5467 RVA: 0x000BD6D4 File Offset: 0x000BBAD4
		public override bool ShouldMeasureTimeNow(Pawn pawn)
		{
			return pawn.InBed() && (HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && (pawn.needs.rest.CurLevel >= 1f || pawn.CurJob.restUntilHealed)));
		}
	}
}
