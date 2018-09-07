using System;

namespace RimWorld
{
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		public Thought_Incestuous()
		{
		}

		public override float OpinionOffset()
		{
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
