using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D0 RID: 464
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x00056414 File Offset: 0x00054814
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
