using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = (Pawn)t;
				if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.Release && !pawn2.Downed && pawn2.Awake())
				{
					IntVec3 c = default(IntVec3);
					if (!RCellFinder.TryFindPrisonerReleaseCell(pawn2, pawn, out c))
					{
						result = null;
					}
					else
					{
						Job job = new Job(JobDefOf.ReleasePrisoner, (Thing)pawn2, c);
						job.count = 1;
						result = job;
					}
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
	}
}
