using System;

namespace Verse.AI
{
	public class ThinkNode_ChancePerHour_Mate : ThinkNode_ChancePerHour
	{
		public ThinkNode_ChancePerHour_Mate()
		{
		}

		protected override float MtbHours(Pawn pawn)
		{
			return pawn.RaceProps.mateMtbHours;
		}
	}
}
