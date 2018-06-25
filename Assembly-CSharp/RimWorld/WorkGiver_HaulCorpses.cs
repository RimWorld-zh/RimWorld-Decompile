using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		public WorkGiver_HaulCorpses()
		{
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!(t is Corpse))
			{
				result = null;
			}
			else
			{
				result = base.JobOnThing(pawn, t, forced);
			}
			return result;
		}
	}
}
