using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BD RID: 445
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x00055ECC File Offset: 0x000542CC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
