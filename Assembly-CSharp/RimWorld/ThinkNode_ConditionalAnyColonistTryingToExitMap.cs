using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnyColonistTryingToExitMap : ThinkNode_Conditional
	{
		protected override bool Satisfied(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			bool result;
			if (mapHeld == null)
			{
				result = false;
			}
			else
			{
				foreach (Pawn item in mapHeld.mapPawns.FreeColonistsSpawned)
				{
					Job curJob = item.CurJob;
					if (curJob != null && curJob.exitMapOnArrival)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
