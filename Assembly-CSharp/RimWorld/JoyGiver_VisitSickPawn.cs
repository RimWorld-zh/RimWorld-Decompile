using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		public override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!InteractionUtility.CanInitiateInteraction(pawn))
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = SickPawnVisitUtility.FindRandomSickPawn(pawn, JoyCategory.Low);
				result = ((pawn2 != null) ? new Job(base.def.jobDef, (Thing)pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2)) : null);
			}
			return result;
		}
	}
}
