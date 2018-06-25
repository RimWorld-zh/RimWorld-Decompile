using System;

namespace Verse.AI
{
	// Token: 0x02000AB6 RID: 2742
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06003D25 RID: 15653 RVA: 0x0020515C File Offset: 0x0020355C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
