using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DA RID: 474
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x0600096E RID: 2414 RVA: 0x00056688 File Offset: 0x00054A88
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
