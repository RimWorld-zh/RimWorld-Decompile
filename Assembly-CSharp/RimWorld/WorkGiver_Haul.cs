using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_Haul : WorkGiver_Scanner
	{
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				result = null;
			}
			else
			{
				Job job = result = HaulAIUtility.HaulToStorageJob(pawn, t);
			}
			return result;
		}
	}
}
