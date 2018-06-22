using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E8 RID: 232
	public class JobGiver_PatientGoToBed : ThinkNode
	{
		// Token: 0x060004FE RID: 1278 RVA: 0x00037A60 File Offset: 0x00035E60
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

		// Token: 0x040002C7 RID: 711
		public bool respectTimetable = true;
	}
}
