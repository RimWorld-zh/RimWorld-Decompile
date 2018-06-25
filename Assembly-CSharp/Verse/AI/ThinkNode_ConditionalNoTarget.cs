using System;

namespace Verse.AI
{
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalNoTarget()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
