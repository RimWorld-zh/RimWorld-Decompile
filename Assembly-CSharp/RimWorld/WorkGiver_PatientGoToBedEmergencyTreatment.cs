using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000152 RID: 338
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x000475C4 File Offset: 0x000459C4
		public override Job NonScanJob(Pawn pawn)
		{
			Job result;
			if (!HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(pawn))
			{
				result = null;
			}
			else
			{
				result = base.NonScanJob(pawn);
			}
			return result;
		}
	}
}
