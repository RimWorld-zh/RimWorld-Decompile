using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000AB3 RID: 2739
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x0400268E RID: 9870
		public List<MentalStateDef> states;

		// Token: 0x06003D20 RID: 15648 RVA: 0x00204DF0 File Offset: 0x002031F0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00204E20 File Offset: 0x00203220
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}
	}
}
