using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C0 RID: 448
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x00055F5C File Offset: 0x0005435C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
