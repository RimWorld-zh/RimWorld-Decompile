using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		public JoyGiver_VisitSickPawn()
		{
		}

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
				if (pawn2 == null)
				{
					result = null;
				}
				else
				{
					result = new Job(this.def.jobDef, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
				}
			}
			return result;
		}
	}
}
