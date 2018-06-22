using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BD RID: 445
	public class ThinkNode_ConditionalGuest : ThinkNode_Conditional
	{
		// Token: 0x0600092F RID: 2351 RVA: 0x00055ED0 File Offset: 0x000542D0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HostFaction != null && !pawn.IsPrisoner;
		}
	}
}
