using System;

namespace RimWorld
{
	// Token: 0x020001FC RID: 508
	public class Thought_Incestuous : Thought_SituationalSocial
	{
		// Token: 0x060009C6 RID: 2502 RVA: 0x00057EEC File Offset: 0x000562EC
		public override float OpinionOffset()
		{
			return LovePartnerRelationUtility.IncestOpinionOffsetFor(this.otherPawn, this.pawn);
		}
	}
}
