using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalStarving : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.needs.food != null && (int)pawn.needs.food.CurCategory >= 3;
		}
	}
}
