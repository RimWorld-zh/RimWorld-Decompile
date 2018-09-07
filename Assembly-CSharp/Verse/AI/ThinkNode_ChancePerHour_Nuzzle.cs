using System;

namespace Verse.AI
{
	public class ThinkNode_ChancePerHour_Nuzzle : ThinkNode_ChancePerHour
	{
		public ThinkNode_ChancePerHour_Nuzzle()
		{
		}

		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.nuzzleMtbHours;
		}
	}
}
