using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C2 RID: 450
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerGuest : ThinkNode_Conditional
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x00055FD8 File Offset: 0x000543D8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && !pawn.guest.IsPrisoner);
		}
	}
}
