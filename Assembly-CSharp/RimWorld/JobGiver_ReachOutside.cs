using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000112 RID: 274
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		// Token: 0x060005A1 RID: 1441 RVA: 0x0003C974 File Offset: 0x0003AD74
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
