using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C1 RID: 449
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06000936 RID: 2358 RVA: 0x00055F88 File Offset: 0x00054388
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
