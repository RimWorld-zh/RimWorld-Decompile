using System;

namespace Verse.AI
{
	// Token: 0x02000AB1 RID: 2737
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x0400268C RID: 9868
		public MentalStateDef state;

		// Token: 0x06003D1A RID: 15642 RVA: 0x00204D24 File Offset: 0x00203124
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00204D54 File Offset: 0x00203154
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MentalStateDef == this.state;
		}
	}
}
