using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		protected abstract int IngestInterval(Pawn pawn);

		protected override Job TryGiveJob(Pawn pawn)
		{
			int num = Find.TickManager.TicksGame - pawn.mindState.lastIngestTick;
			Job result;
			if (num > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					result = job;
					goto IL_0043;
				}
			}
			result = null;
			goto IL_0043;
			IL_0043:
			return result;
		}

		private Job IngestJob(Pawn pawn)
		{
			Thing thing = this.BestIngestTarget(pawn);
			Job result;
			if (thing == null)
			{
				result = null;
			}
			else
			{
				ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
				Job job = new Job(JobDefOf.Ingest, thing);
				job.count = finalIngestibleDef.ingestible.maxNumToIngestAtOnce;
				job.ignoreForbidden = this.IgnoreForbid(pawn);
				job.overeat = true;
				result = job;
			}
			return result;
		}

		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
