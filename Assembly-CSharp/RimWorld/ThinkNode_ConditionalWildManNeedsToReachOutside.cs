using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E9 RID: 489
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x00056C50 File Offset: 0x00055050
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsWildMan() && !pawn.mindState.wildManEverReachedOutside;
		}
	}
}
