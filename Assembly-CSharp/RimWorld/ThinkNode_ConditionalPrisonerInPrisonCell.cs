using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalPrisonerInPrisonCell : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			bool result;
			if (!pawn.IsPrisoner)
			{
				result = false;
			}
			else
			{
				Room room = pawn.GetRoom(RegionType.Set_Passable);
				result = (room != null && room.isPrisonCell);
			}
			return result;
		}
	}
}
