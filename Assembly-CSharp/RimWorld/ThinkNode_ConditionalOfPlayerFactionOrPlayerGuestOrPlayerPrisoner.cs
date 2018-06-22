using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C3 RID: 451
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuestOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x0600093B RID: 2363 RVA: 0x00056030 File Offset: 0x00054430
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer;
		}
	}
}
