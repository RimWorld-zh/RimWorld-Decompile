using System;

namespace Verse.AI
{
	// Token: 0x02000AB2 RID: 2738
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x04002693 RID: 9875
		public MentalStateDef state;

		// Token: 0x06003D1A RID: 15642 RVA: 0x00205004 File Offset: 0x00203404
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00205034 File Offset: 0x00203434
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MentalStateDef == this.state;
		}
	}
}
