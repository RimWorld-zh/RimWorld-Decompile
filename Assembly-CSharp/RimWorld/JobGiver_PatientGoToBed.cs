using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PatientGoToBed : ThinkNode
	{
		public bool respectTimetable = true;

		public JobGiver_PatientGoToBed()
		{
		}

		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			if (!HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				result = ThinkResult.NoJob;
			}
			else if (this.respectTimetable && RestUtility.TimetablePreventsLayDown(pawn) && !HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn) && !HealthAIUtility.ShouldBeTendedNowByPlayer(pawn))
			{
				result = ThinkResult.NoJob;
			}
			else if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				result = ThinkResult.NoJob;
			}
			else
			{
				Thing thing = RestUtility.FindPatientBedFor(pawn);
				if (thing == null)
				{
					result = ThinkResult.NoJob;
				}
				else
				{
					Job job = new Job(JobDefOf.LayDown, thing);
					result = new ThinkResult(job, this, null, false);
				}
			}
			return result;
		}
	}
}
