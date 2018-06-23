using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000AB RID: 171
	public class JobGiver_FleePotentialExplosion : ThinkNode_JobGiver
	{
		// Token: 0x0400027C RID: 636
		public const float FleeDist = 9f;

		// Token: 0x06000426 RID: 1062 RVA: 0x00031A80 File Offset: 0x0002FE80
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
