using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BD RID: 445
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x06000931 RID: 2353 RVA: 0x00055EBC File Offset: 0x000542BC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
