using System;

namespace Verse.AI
{
	// Token: 0x02000AB4 RID: 2740
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06003D1E RID: 15646 RVA: 0x00204930 File Offset: 0x00202D30
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x00204960 File Offset: 0x00202D60
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}

		// Token: 0x04002691 RID: 9873
		public Type stateClass;
	}
}
