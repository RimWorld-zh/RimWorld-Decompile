using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_FleePotentialExplosion : ThinkNode_JobGiver
	{
		public const float FleeDist = 9f;

		public JobGiver_FleePotentialExplosion()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.RaceProps.intelligence < Intelligence.Humanlike)
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
				IntVec3 c;
				if ((float)(pawn.Position - knownExploder.Position).LengthHorizontalSquared > 81f)
				{
					result = null;
				}
				else if (!RCellFinder.TryFindDirectFleeDestination(knownExploder.Position, 9f, pawn, out c))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Goto, c)
					{
						locomotionUrgency = LocomotionUrgency.Sprint
					};
				}
			}
			return result;
		}
	}
}
