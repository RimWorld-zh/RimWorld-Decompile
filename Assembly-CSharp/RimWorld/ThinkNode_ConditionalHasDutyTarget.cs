using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DA RID: 474
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x0600096B RID: 2411 RVA: 0x00056698 File Offset: 0x00054A98
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
