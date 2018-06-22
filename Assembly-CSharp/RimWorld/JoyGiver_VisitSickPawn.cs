using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000105 RID: 261
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x0003B668 File Offset: 0x00039A68
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
