using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		public ThoughtWorker_Incestuous()
		{
		}

		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				result = false;
			}
			else if (LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) == 0f)
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
