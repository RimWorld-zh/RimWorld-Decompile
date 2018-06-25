using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalCannotReachMapEdge()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
