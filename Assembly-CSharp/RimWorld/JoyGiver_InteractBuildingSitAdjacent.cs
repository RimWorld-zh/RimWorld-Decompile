using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F3 RID: 243
	public class JoyGiver_InteractBuildingSitAdjacent : JoyGiver_InteractBuilding
	{
		// Token: 0x06000524 RID: 1316 RVA: 0x00038CDC File Offset: 0x000370DC
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

		// Token: 0x040002CD RID: 717
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
