using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001DB RID: 475
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		// Token: 0x06000970 RID: 2416 RVA: 0x000566D0 File Offset: 0x00054AD0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
