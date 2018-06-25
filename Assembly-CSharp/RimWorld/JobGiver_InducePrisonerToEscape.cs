using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200010E RID: 270
	public class JobGiver_InducePrisonerToEscape : ThinkNode_JobGiver
	{
		// Token: 0x06000594 RID: 1428 RVA: 0x0003C554 File Offset: 0x0003A954
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn pawn2 = JailbreakerMentalStateUtility.FindPrisoner(pawn);
			Job result;
			if (pawn2 == null || !pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.InducePrisonerToEscape, pawn2)
				{
					interaction = InteractionDefOf.SparkJailbreak
				};
			}
			return result;
		}
	}
}
