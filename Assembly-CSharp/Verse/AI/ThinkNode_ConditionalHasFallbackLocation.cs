using System;

namespace Verse.AI
{
	// Token: 0x02000AB3 RID: 2739
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06003D21 RID: 15649 RVA: 0x00204D50 File Offset: 0x00203150
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
