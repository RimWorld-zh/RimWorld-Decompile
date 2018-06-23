using System;

namespace Verse.AI
{
	// Token: 0x02000AAF RID: 2735
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x0400268B RID: 9867
		public MentalStateDef state;

		// Token: 0x06003D16 RID: 15638 RVA: 0x00204BF8 File Offset: 0x00202FF8
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00204C28 File Offset: 0x00203028
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MentalStateDef == this.state;
		}
	}
}
