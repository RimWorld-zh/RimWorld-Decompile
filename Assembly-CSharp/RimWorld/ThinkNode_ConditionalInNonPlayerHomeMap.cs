using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E3 RID: 483
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x0600097F RID: 2431 RVA: 0x00056A1C File Offset: 0x00054E1C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
