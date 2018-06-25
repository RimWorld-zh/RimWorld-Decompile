using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		public const float VictimSearchRadiusInitial = 8f;

		private const float VictimSearchRadiusOngoing = 18f;

		public JobGiver_Kidnap()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			Job result;
			Pawn t;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				result = null;
			}
			else if (KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 18f, out t, null) && !GenAI.InDangerousCombat(pawn))
			{
				result = new Job(JobDefOf.Kidnap)
				{
					targetA = t,
					targetB = c,
					count = 1
				};
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
