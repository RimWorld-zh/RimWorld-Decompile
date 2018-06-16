using System;

namespace Verse.AI
{
	// Token: 0x02000AB4 RID: 2740
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x06003D1C RID: 15644 RVA: 0x0020485C File Offset: 0x00202C5C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x0020488C File Offset: 0x00202C8C
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsAssignableFrom(mentalState.GetType());
		}

		// Token: 0x04002691 RID: 9873
		public Type stateClass;
	}
}
