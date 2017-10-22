using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Clear();
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.AddRange(GenAdjFast.AdjacentCellsCardinal(t));
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Shuffle();
			Thing thing = null;
			for (int i = 0; i < JoyGiver_InteractBuildingSitAdjacent.tmpCells.Count; i++)
			{
				IntVec3 c = JoyGiver_InteractBuildingSitAdjacent.tmpCells[i];
				if (!c.IsForbidden(pawn))
				{
					Building edifice = c.GetEdifice(pawn.Map);
					if (edifice != null && edifice.def.building.isSittable && pawn.CanReserve((Thing)edifice, 1, -1, null, false))
					{
						thing = edifice;
						break;
					}
				}
			}
			return (thing != null) ? new Job(base.def.jobDef, t, thing) : null;
		}
	}
}
