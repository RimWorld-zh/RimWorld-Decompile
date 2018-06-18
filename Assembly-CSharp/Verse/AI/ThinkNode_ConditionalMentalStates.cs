using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000AB5 RID: 2741
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x06003D21 RID: 15649 RVA: 0x002049A0 File Offset: 0x00202DA0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x002049D0 File Offset: 0x00202DD0
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x04002692 RID: 9874
		public List<MentalStateDef> states;
	}
}
