using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000DD RID: 221
	public class JobGiver_TakeWoundedGuest : ThinkNode_JobGiver
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x000363AC File Offset: 0x000347AC
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			Job result;
			if (!RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = KidnapAIUtility.ReachableWoundedGuest(pawn);
				if (pawn2 == null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Kidnap)
					{
						targetA = pawn2,
						targetB = c,
						count = 1
					};
				}
			}
			return result;
		}
	}
}
