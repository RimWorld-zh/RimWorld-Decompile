using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_UnloadMyLordCarriers : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			Lord lord;
			int i;
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				result = null;
			}
			else
			{
				lord = pawn.GetLord();
				if (lord == null)
				{
					result = null;
				}
				else
				{
					for (i = 0; i < lord.ownedPawns.Count; i++)
					{
						if (UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, lord.ownedPawns[i], false))
							goto IL_005b;
					}
					result = null;
				}
			}
			goto IL_0098;
			IL_0098:
			return result;
			IL_005b:
			result = new Job(JobDefOf.UnloadInventory, (Thing)lord.ownedPawns[i]);
			goto IL_0098;
		}
	}
}
