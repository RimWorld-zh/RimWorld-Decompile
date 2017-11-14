using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_FleePotentialExplosion : ThinkNode_JobGiver
	{
		public const float FleeDist = 9f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			if ((int)pawn.RaceProps.intelligence < 2)
			{
				return null;
			}
			if (pawn.mindState.knownExploder == null)
			{
				return null;
			}
			if (!pawn.mindState.knownExploder.Spawned)
			{
				pawn.mindState.knownExploder = null;
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			Thing knownExploder = pawn.mindState.knownExploder;
			if ((float)(pawn.Position - knownExploder.Position).LengthHorizontalSquared > 81.0)
			{
				return null;
			}
			IntVec3 c = default(IntVec3);
			if (!RCellFinder.TryFindDirectFleeDestination(knownExploder.Position, 9f, pawn, out c))
			{
				return null;
			}
			Job job = new Job(JobDefOf.Goto, c);
			job.locomotionUrgency = LocomotionUrgency.Sprint;
			return job;
		}
	}
}
