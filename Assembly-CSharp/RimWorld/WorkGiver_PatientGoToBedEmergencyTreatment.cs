using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000152 RID: 338
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x000475DC File Offset: 0x000459DC
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
