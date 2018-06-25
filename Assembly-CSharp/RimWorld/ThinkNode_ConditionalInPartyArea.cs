using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E2 RID: 482
	public class ThinkNode_ConditionalInPartyArea : ThinkNode_Conditional
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x000569C0 File Offset: 0x00054DC0
		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (pawn.mindState.duty == null)
			{
				result = false;
			}
			else
			{
				IntVec3 cell = pawn.mindState.duty.focus.Cell;
				result = PartyUtility.InPartyArea(pawn.Position, cell, pawn.Map);
			}
			return result;
		}
	}
}
