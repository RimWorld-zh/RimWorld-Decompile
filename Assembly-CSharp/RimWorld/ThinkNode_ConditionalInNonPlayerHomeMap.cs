using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E3 RID: 483
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x00056A20 File Offset: 0x00054E20
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
