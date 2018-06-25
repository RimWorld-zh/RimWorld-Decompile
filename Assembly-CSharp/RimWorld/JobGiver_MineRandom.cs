using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000A2 RID: 162
	public class JobGiver_MineRandom : ThinkNode_JobGiver
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00030744 File Offset: 0x0002EB44
		protected override Job TryGiveJob(Pawn pawn)
		{
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			Job result;
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
						if (c.InBounds(pawn.Map))
						{
							Building edifice = c.GetEdifice(pawn.Map);
							if (edifice != null && (edifice.def.passability == Traversability.Impassable || edifice.def.IsDoor) && edifice.def.size == IntVec2.One && edifice.def != ThingDefOf.CollapsedRocks && pawn.CanReserve(edifice, 1, -1, null, false))
							{
								return new Job(JobDefOf.Mine, edifice)
								{
									ignoreDesignations = true
								};
							}
						}
					}
				}
				result = null;
			}
			return result;
		}
	}
}
