using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.Release && !pawn2.Downed && pawn2.Awake())
			{
				IntVec3 c = default(IntVec3);
				if (!RCellFinder.TryFindPrisonerReleaseCell(pawn2, pawn, out c))
				{
					return null;
				}
				Job job = new Job(JobDefOf.ReleasePrisoner, (Thing)pawn2, c);
				job.count = 1;
				return job;
			}
			return null;
		}
	}
}
