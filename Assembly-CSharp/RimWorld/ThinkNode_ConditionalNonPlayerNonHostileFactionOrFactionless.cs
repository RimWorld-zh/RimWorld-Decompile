using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C7 RID: 455
	public class ThinkNode_ConditionalNonPlayerNonHostileFactionOrFactionless : ThinkNode_Conditional
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x0005614C File Offset: 0x0005454C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == null || (pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer));
		}
	}
}
