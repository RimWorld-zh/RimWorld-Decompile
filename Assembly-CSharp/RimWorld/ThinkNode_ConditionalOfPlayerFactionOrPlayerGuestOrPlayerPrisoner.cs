using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C3 RID: 451
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x0005601C File Offset: 0x0005441C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
