using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000153 RID: 339
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x00047570 File Offset: 0x00045970
		public override Job NonScanJob(Pawn pawn)
		{
			ThinkResult thinkResult = WorkGiver_PatientGoToBedRecuperate.jgp.TryIssueJobPackage(pawn, default(JobIssueParams));
			Job result;
			if (thinkResult.IsValid)
			{
				result = thinkResult.Job;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400032E RID: 814
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};
	}
}
