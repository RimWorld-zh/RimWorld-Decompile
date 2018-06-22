using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C1 RID: 449
	public class ThinkNode_ConditionalOfPlayerFactionOrPlayerPrisoner : ThinkNode_Conditional
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x00055F8C File Offset: 0x0005438C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer || (pawn.HostFaction == Faction.OfPlayer && pawn.guest.IsPrisoner);
		}
	}
}
