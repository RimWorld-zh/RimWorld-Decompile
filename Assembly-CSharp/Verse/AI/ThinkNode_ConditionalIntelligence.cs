using System;

namespace Verse.AI
{
	// Token: 0x02000AB4 RID: 2740
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x0400268E RID: 9870
		public Intelligence minIntelligence = Intelligence.ToolUser;

		// Token: 0x06003D23 RID: 15651 RVA: 0x00204D9C File Offset: 0x0020319C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00204DCC File Offset: 0x002031CC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}
	}
}
