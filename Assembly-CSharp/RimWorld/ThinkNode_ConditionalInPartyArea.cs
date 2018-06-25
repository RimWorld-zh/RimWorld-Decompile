using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalInPartyArea : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalInPartyArea()
		{
		}

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
