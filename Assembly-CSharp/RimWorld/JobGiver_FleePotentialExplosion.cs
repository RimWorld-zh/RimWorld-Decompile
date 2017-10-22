using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_FleePotentialExplosion : ThinkNode_JobGiver
	{
		public const float FleeDist = 9f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if ((int)pawn.RaceProps.intelligence < 2)
			{
				result = null;
			}
			else if (pawn.mindState.knownExploder == null)
			{
				result = null;
			}
			else if (!pawn.mindState.knownExploder.Spawned)
			{
				pawn.mindState.knownExploder = null;
				result = null;
			}
			else if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				result = null;
			}
			else
			{
				Thing knownExploder = pawn.mindState.knownExploder;
				IntVec3 c = default(IntVec3);
				if ((float)(pawn.Position - knownExploder.Position).LengthHorizontalSquared > 81.0)
				{
					result = null;
				}
				else if (!RCellFinder.TryFindDirectFleeDestination(knownExploder.Position, 9f, pawn, out c))
				{
					result = null;
				}
				else
				{
					Job job = new Job(JobDefOf.Goto, c);
					job.locomotionUrgency = LocomotionUrgency.Sprint;
					result = job;
				}
			}
			return result;
		}
	}
}
