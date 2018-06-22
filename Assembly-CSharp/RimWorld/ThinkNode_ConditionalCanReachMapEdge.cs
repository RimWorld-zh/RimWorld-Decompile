using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D6 RID: 470
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x00056580 File Offset: 0x00054980
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge();
		}
	}
}
