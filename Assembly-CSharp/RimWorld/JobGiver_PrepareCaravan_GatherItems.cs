using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000C3 RID: 195
	public class JobGiver_PrepareCaravan_GatherItems : ThinkNode_JobGiver
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x00033D7C File Offset: 0x0003217C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				result = null;
			}
			else
			{
				Lord lord = pawn.GetLord();
				Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lord);
				if (thing == null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.PrepareCaravan_GatherItems, thing)
					{
						lord = lord
					};
				}
			}
			return result;
		}
	}
}
