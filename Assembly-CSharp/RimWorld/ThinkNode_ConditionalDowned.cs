using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001B9 RID: 441
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x06000926 RID: 2342 RVA: 0x00055DE4 File Offset: 0x000541E4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
