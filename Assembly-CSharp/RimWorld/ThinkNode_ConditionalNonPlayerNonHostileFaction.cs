using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C6 RID: 454
	public class ThinkNode_ConditionalNonPlayerNonHostileFaction : ThinkNode_Conditional
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x000560EC File Offset: 0x000544EC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
