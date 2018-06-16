using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E9 RID: 489
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		// Token: 0x06000990 RID: 2448 RVA: 0x00056C40 File Offset: 0x00055040
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsWildMan() && !pawn.mindState.wildManEverReachedOutside;
		}
	}
}
