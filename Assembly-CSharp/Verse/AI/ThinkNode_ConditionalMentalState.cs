using System;

namespace Verse.AI
{
	// Token: 0x02000AB3 RID: 2739
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x06003D1B RID: 15643 RVA: 0x002048D4 File Offset: 0x00202CD4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00204904 File Offset: 0x00202D04
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MentalStateDef == this.state;
		}

		// Token: 0x04002690 RID: 9872
		public MentalStateDef state;
	}
}
