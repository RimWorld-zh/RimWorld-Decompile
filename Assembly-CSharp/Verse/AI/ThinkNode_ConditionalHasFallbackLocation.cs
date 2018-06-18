using System;

namespace Verse.AI
{
	// Token: 0x02000AB7 RID: 2743
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06003D26 RID: 15654 RVA: 0x00204A2C File Offset: 0x00202E2C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
