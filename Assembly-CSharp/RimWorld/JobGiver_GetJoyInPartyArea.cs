using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000CA RID: 202
	public class JobGiver_GetJoyInPartyArea : JobGiver_GetJoy
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x00034B98 File Offset: 0x00032F98
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			Job result;
			if (pawn.mindState.duty == null)
			{
				result = null;
			}
			else if (pawn.needs.joy == null)
			{
				result = null;
			}
			else
			{
				float curLevelPercentage = pawn.needs.joy.CurLevelPercentage;
				if (curLevelPercentage > 0.92f)
				{
					result = null;
				}
				else
				{
					IntVec3 cell = pawn.mindState.duty.focus.Cell;
					result = def.Worker.TryGiveJobInPartyArea(pawn, cell);
				}
			}
			return result;
		}
	}
}
