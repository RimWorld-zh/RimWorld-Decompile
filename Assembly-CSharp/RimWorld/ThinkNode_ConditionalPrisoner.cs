using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalPrisoner : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalPrisoner()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsPrisoner;
		}
	}
}
