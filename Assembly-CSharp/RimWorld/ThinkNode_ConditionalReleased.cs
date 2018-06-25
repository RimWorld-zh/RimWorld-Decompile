using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BE RID: 446
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x00055F00 File Offset: 0x00054300
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
