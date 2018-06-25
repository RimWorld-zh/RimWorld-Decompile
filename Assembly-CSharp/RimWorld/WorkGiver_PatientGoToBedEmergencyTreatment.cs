using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		public WorkGiver_PatientGoToBedEmergencyTreatment()
		{
		}

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
