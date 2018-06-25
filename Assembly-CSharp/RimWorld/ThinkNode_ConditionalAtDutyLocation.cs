using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DC RID: 476
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x0600096F RID: 2415 RVA: 0x00056730 File Offset: 0x00054B30
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
