using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class ThinkNode_ConditionalHasLord : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalHasLord()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() != null;
		}
	}
}
