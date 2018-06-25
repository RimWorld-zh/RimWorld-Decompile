using System;

namespace RimWorld
{
	// Token: 0x020001FC RID: 508
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x060009C3 RID: 2499 RVA: 0x00057F2C File Offset: 0x0005632C
		public override float OpinionOffset()
		{
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
