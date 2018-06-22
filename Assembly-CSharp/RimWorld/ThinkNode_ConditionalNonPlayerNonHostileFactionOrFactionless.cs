using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C7 RID: 455
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x00056150 File Offset: 0x00054550
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
