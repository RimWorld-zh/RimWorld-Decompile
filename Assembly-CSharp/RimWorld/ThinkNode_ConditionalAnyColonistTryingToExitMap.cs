using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAnyColonistTryingToExitMap : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalAnyColonistTryingToExitMap()
		{
		}

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
				foreach (Pawn pawn2 in mapHeld.mapPawns.FreeColonistsSpawned)
				{
					Job curJob = pawn2.CurJob;
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
