using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		public const float VictimSearchRadiusInitial = 8f;

		private const float VictimSearchRadiusOngoing = 18f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c = default(IntVec3);
			Job result;
			Pawn t = default(Pawn);
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				result = null;
			}
			else if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 18f, out t, (List<Thing>)null) && !GenAI.InDangerousCombat(pawn))
			{
				Job job = new Job(JobDefOf.Kidnap);
				job.targetA = (Thing)t;
				job.targetB = c;
				job.count = 1;
				result = job;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
