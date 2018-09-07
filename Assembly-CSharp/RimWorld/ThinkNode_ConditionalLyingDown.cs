using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalLyingDown : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalLyingDown()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetPosture().Laying();
		}
	}
}
