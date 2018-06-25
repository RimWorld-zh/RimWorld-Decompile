using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A57 RID: 2647
	public static class JobUtility
	{
		// Token: 0x04002551 RID: 9553
		private static bool startingErrorRecoverJob = false;

		// Token: 0x06003AF1 RID: 15089 RVA: 0x001F4908 File Offset: 0x001F2D08
		public static void TryStartErrorRecoverJob(Pawn pawn, string message, Exception exception = null, JobDriver concreteDriver = null)
		{
			string text = message;
			JobUtility.AppendVarsInfoToDebugMessage(pawn, ref text, concreteDriver);
			if (exception != null)
			{
				text = text + "\n" + exception;
			}
			Log.Error(text, false);
			if (pawn.jobs != null)
			{
				if (pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Errored, false);
				}
				if (JobUtility.startingErrorRecoverJob)
				{
					Log.Error("An error occurred while starting an error recover job. We have to stop now to avoid infinite loops. This means that the pawn is now jobless which can cause further bugs. pawn=" + pawn.ToStringSafe<Pawn>(), false);
				}
				else
				{
					JobUtility.startingErrorRecoverJob = true;
					try
					{
						pawn.jobs.StartJob(new Job(JobDefOf.Wait, 150, false), JobCondition.None, null, false, true, null, null, false);
					}
					finally
					{
						JobUtility.startingErrorRecoverJob = false;
					}
				}
			}
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x001F49E0 File Offset: 0x001F2DE0
		private static void AppendVarsInfoToDebugMessage(Pawn pawn, ref string msg, JobDriver concreteDriver)
		{
			if (concreteDriver != null)
			{
				string text = msg;
				msg = string.Concat(new object[]
				{
					text,
					" driver=",
					concreteDriver.GetType().Name,
					" (toilIndex=",
					concreteDriver.CurToilIndex,
					")"
				});
				if (concreteDriver.job != null)
				{
					msg = msg + " driver.job=(" + concreteDriver.job.ToStringSafe<Job>() + ")";
				}
			}
			else if (pawn.jobs != null)
			{
				if (pawn.jobs.curDriver != null)
				{
					string text = msg;
					msg = string.Concat(new object[]
					{
						text,
						" curDriver=",
						pawn.jobs.curDriver.GetType().Name,
						" (toilIndex=",
						pawn.jobs.curDriver.CurToilIndex,
						")"
					});
				}
				if (pawn.jobs.curJob != null)
				{
					msg = msg + " curJob=(" + pawn.jobs.curJob.ToStringSafe<Job>() + ")";
				}
			}
			if (pawn.mindState != null)
			{
				msg = msg + " lastJobGiver=" + pawn.mindState.lastJobGiver.ToStringSafe<ThinkNode>();
			}
		}
	}
}
