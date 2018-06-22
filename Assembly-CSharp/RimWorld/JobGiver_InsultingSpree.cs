using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200010F RID: 271
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x0003C59C File Offset: 0x0003A99C
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			Job result;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.Insult, mentalState_InsultingSpree.target);
			}
			return result;
		}
	}
}
