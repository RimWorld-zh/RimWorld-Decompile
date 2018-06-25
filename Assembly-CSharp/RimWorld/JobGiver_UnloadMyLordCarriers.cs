using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000C6 RID: 198
	public class JobGiver_UnloadMyLordCarriers : ThinkNode_JobGiver
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x00034228 File Offset: 0x00032628
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
				if (lord == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < lord.ownedPawns.Count; i++)
					{
						if (UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, lord.ownedPawns[i], false))
						{
							return new Job(JobDefOf.UnloadInventory, lord.ownedPawns[i]);
						}
					}
					result = null;
				}
			}
			return result;
		}
	}
}
