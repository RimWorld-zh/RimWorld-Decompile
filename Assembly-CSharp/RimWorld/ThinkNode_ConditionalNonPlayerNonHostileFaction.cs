using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C6 RID: 454
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x06000942 RID: 2370 RVA: 0x00056100 File Offset: 0x00054500
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
