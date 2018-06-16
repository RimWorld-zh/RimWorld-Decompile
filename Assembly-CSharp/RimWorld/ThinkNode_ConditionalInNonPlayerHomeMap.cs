using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001E3 RID: 483
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x00056A0C File Offset: 0x00054E0C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
