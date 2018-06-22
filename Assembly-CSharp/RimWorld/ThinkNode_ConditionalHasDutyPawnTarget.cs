using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DB RID: 475
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x0600096E RID: 2414 RVA: 0x000566E4 File Offset: 0x00054AE4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
