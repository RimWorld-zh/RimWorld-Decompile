using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C1 RID: 449
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x00055F78 File Offset: 0x00054378
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
