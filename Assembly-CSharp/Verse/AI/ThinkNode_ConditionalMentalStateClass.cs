using System;

namespace Verse.AI
{
	// Token: 0x02000AB2 RID: 2738
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x0400268D RID: 9869
		public Type stateClass;

		// Token: 0x06003D1D RID: 15645 RVA: 0x00204D80 File Offset: 0x00203180
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x00204DB0 File Offset: 0x002031B0
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}
	}
}
