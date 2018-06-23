using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000AB1 RID: 2737
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x0400268D RID: 9869
		public List<MentalStateDef> states;

		// Token: 0x06003D1C RID: 15644 RVA: 0x00204CC4 File Offset: 0x002030C4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00204CF4 File Offset: 0x002030F4
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}
	}
}
