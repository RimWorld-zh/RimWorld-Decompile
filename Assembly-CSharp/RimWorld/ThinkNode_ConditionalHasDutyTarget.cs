using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DA RID: 474
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x0600096C RID: 2412 RVA: 0x0005669C File Offset: 0x00054A9C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
