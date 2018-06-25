using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001D6 RID: 470
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06000962 RID: 2402 RVA: 0x0005657C File Offset: 0x0005497C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge();
		}
	}
}
