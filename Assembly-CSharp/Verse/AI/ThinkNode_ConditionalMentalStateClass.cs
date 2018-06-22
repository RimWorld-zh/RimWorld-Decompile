using System;

namespace Verse.AI
{
	// Token: 0x02000AB0 RID: 2736
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06003D19 RID: 15641 RVA: 0x00204C54 File Offset: 0x00203054
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x00204C84 File Offset: 0x00203084
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}

		// Token: 0x0400268C RID: 9868
		public Type stateClass;
	}
}
