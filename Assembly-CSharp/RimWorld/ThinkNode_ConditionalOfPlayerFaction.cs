using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C0 RID: 448
	public class ThinkNode_ConditionalOfPlayerFaction : ThinkNode_Conditional
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x00055F4C File Offset: 0x0005434C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Faction == Faction.OfPlayer;
		}
	}
}
