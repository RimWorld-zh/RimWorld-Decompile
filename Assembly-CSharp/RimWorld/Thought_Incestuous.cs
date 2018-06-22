using System;

namespace RimWorld
{
	// Token: 0x020001FC RID: 508
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x00057F30 File Offset: 0x00056330
		public override float OpinionOffset()
		{
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
