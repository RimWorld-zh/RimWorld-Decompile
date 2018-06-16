using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C7 RID: 455
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x0005613C File Offset: 0x0005453C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
