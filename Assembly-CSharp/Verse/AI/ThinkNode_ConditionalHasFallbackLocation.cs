using System;

namespace Verse.AI
{
	// Token: 0x02000AB5 RID: 2741
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06003D25 RID: 15653 RVA: 0x00204E7C File Offset: 0x0020327C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
