using System;

namespace Verse.AI
{
	// Token: 0x02000AB7 RID: 2743
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06003D24 RID: 15652 RVA: 0x00204958 File Offset: 0x00202D58
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
