using System;

namespace Verse.AI
{
	// Token: 0x02000AB8 RID: 2744
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x06003D28 RID: 15656 RVA: 0x00204A78 File Offset: 0x00202E78
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00204AA8 File Offset: 0x00202EA8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}

		// Token: 0x04002693 RID: 9875
		public Intelligence minIntelligence = Intelligence.ToolUser;
	}
}
