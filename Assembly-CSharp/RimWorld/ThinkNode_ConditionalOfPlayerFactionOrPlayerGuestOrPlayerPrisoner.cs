using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C3 RID: 451
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x0005602C File Offset: 0x0005442C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
