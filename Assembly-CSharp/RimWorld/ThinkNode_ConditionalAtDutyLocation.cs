using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DC RID: 476
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x06000970 RID: 2416 RVA: 0x00056734 File Offset: 0x00054B34
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
