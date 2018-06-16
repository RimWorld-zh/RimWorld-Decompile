using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DC RID: 476
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x00056720 File Offset: 0x00054B20
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
