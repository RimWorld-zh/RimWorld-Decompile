using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalColonist()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
