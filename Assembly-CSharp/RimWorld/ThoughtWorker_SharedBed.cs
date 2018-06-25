using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		public ThoughtWorker_SharedBed()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else
			{
				result = (LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null);
			}
			return result;
		}
	}
}
