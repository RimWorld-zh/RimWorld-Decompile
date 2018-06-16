using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D7 RID: 471
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x00056590 File Offset: 0x00054990
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
