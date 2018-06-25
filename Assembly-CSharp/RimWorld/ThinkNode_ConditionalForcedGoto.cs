using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalForcedGoto()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
