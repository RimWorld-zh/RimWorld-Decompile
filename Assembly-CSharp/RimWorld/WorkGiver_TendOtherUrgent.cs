using System;
using Verse;

namespace RimWorld
{
	public class WorkGiver_TendOtherUrgent : WorkGiver_TendOther
	{
		public WorkGiver_TendOtherUrgent()
		{
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return base.HasJobOnThing(pawn, t, forced) && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent((Pawn)t);
		}
	}
}
