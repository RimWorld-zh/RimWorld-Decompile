using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalReleased()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
