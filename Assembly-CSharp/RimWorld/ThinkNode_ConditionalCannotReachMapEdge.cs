using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D7 RID: 471
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06000965 RID: 2405 RVA: 0x000565A4 File Offset: 0x000549A4
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
