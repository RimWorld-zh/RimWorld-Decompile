using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_MineRandom : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			Job result;
			Building edifice;
			if (region == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < 40; i++)
				{
					IntVec3 randomCell = region.RandomCell;
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c = randomCell + GenAdj.CardinalDirections[j];
						edifice = c.GetEdifice(pawn.Map);
						if (edifice != null && (edifice.def.passability == Traversability.Impassable || edifice.def.IsDoor) && edifice.def.size == IntVec2.One && edifice.def != ThingDefOf.CollapsedRocks && pawn.CanReserve((Thing)edifice, 1, -1, null, false))
							goto IL_00c2;
					}
				}
				result = null;
			}
			goto IL_0109;
			IL_00c2:
			Job job = new Job(JobDefOf.Mine, (Thing)edifice);
			job.ignoreDesignations = true;
			result = job;
			goto IL_0109;
			IL_0109:
			return result;
		}
	}
}
