using System;

namespace Verse.AI
{
	// Token: 0x02000AB6 RID: 2742
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x0400268F RID: 9871
		public Intelligence minIntelligence = Intelligence.ToolUser;

		// Token: 0x06003D27 RID: 15655 RVA: 0x00204EC8 File Offset: 0x002032C8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00204EF8 File Offset: 0x002032F8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}
	}
}
