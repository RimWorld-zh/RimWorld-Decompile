using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D0 RID: 464
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x06000957 RID: 2391 RVA: 0x00056428 File Offset: 0x00054828
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
