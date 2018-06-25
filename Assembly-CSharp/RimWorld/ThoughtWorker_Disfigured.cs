using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		public ThoughtWorker_Disfigured()
		{
		}

		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				result = false;
			}
			else if (!RelationsUtility.IsDisfigured(other))
			{
				result = false;
			}
			else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
