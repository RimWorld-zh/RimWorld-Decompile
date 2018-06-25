using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalExitTimedOut()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
