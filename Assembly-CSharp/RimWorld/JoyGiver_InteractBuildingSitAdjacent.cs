using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		public JoyGiver_InteractBuildingSitAdjacent()
		{
		}

		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Clear();
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.AddRange(GenAdjFast.AdjacentCellsCardinal(t));
			JoyGiver_InteractBuildingSitAdjacent.tmpCells.Shuffle<IntVec3>();
			Thing thing = null;
			for (int i = 0; i < JoyGiver_InteractBuildingSitAdjacent.tmpCells.Count; i++)
			{
				IntVec3 c = JoyGiver_InteractBuildingSitAdjacent.tmpCells[i];
				if (!c.IsForbidden(pawn))
				{
					Building edifice = c.GetEdifice(pawn.Map);
					if (edifice != null && edifice.def.building.isSittable && pawn.CanReserve(edifice, 1, -1, null, false))
					{
						thing = edifice;
						break;
					}
				}
			}
			Job result;
			if (thing == null)
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, t, thing);
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JoyGiver_InteractBuildingSitAdjacent()
		{
		}
	}
}
