using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_PatientGoToBedEmergencyTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		public override Job NonScanJob(Pawn pawn)
		{
			return HealthAIUtility.ShouldBeTendedNowUrgent(pawn) ? base.NonScanJob(pawn) : null;
		}
	}
}
