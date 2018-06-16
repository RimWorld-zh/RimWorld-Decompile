using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000AC RID: 172
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x00031BA0 File Offset: 0x0002FFA0
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x06000429 RID: 1065
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x0600042A RID: 1066 RVA: 0x00031BBC File Offset: 0x0002FFBC
		protected override Job TryGiveJob(Pawn pawn)
		{
			int num = Find.TickManager.TicksGame - pawn.mindState.lastIngestTick;
			if (num > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00031C10 File Offset: 0x00030010
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
				result = new Job(JobDefOf.Ingest, thing)
				{
					count = finalIngestibleDef.ingestible.maxNumToIngestAtOnce,
					ignoreForbidden = this.IgnoreForbid(pawn),
					overeat = true
				};
			}
			return result;
		}

		// Token: 0x0600042C RID: 1068
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
