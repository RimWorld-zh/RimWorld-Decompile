using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		public JobGiver_ReachOutside()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			Job result;
			IntVec3 intVec;
			if (room.PsychologicallyOutdoors && room.TouchesMapEdge)
			{
				result = null;
			}
			else if (!pawn.CanReachMapEdge())
			{
				result = null;
			}
			else if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out intVec))
			{
				result = null;
			}
			else if (intVec == pawn.Position)
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.Goto, intVec);
			}
			return result;
		}
	}
}
