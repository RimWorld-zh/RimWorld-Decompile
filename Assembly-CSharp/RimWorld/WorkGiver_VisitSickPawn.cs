using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_VisitSickPawn : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return !InteractionUtility.CanInitiateInteraction(pawn);
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return null;
			}
			if (!SickPawnVisitUtility.CanVisit(pawn, pawn2, JoyCategory.VeryLow))
			{
				return null;
			}
			Job job = new Job(JobDefOf.VisitSickPawn, (Thing)pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
			job.ignoreJoyTimeAssignment = true;
			return job;
		}
	}
}
