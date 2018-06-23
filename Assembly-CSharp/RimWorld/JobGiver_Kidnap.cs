using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D3 RID: 211
	public class JobGiver_Kidnap : ThinkNode_JobGiver
	{
		// Token: 0x040002A4 RID: 676
		public const float VictimSearchRadiusInitial = 8f;

		// Token: 0x040002A5 RID: 677
		private const float VictimSearchRadiusOngoing = 18f;

		// Token: 0x060004BA RID: 1210 RVA: 0x000353B4 File Offset: 0x000337B4
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
