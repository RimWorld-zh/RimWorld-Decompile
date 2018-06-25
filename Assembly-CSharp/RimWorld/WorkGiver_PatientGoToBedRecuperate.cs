using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000153 RID: 339
	public class WorkGiver_PatientGoToBedRecuperate : WorkGiver
	{
		// Token: 0x0400032F RID: 815
		private static JobGiver_PatientGoToBed jgp = new JobGiver_PatientGoToBed
		{
			respectTimetable = false
		};

		// Token: 0x060006FC RID: 1788 RVA: 0x00047558 File Offset: 0x00045958
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
	}
}
