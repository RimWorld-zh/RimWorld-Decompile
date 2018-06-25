using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DB RID: 475
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x0600096D RID: 2413 RVA: 0x000566E0 File Offset: 0x00054AE0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
