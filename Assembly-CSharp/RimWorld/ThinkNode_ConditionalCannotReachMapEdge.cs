using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D7 RID: 471
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x000565A0 File Offset: 0x000549A0
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
