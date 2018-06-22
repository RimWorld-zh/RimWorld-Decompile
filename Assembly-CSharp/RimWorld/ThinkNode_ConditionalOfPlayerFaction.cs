using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C0 RID: 448
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06000935 RID: 2357 RVA: 0x00055F60 File Offset: 0x00054360
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
