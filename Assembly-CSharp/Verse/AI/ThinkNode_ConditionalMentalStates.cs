using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000AB5 RID: 2741
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x06003D1F RID: 15647 RVA: 0x002048CC File Offset: 0x00202CCC
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x002048FC File Offset: 0x00202CFC
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x04002692 RID: 9874
		public List<MentalStateDef> states;
	}
}
