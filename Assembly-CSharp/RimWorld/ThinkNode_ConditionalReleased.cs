using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BE RID: 446
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x06000933 RID: 2355 RVA: 0x00055EF0 File Offset: 0x000542F0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
