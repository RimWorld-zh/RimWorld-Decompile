using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CC RID: 460
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x0600094F RID: 2383 RVA: 0x00056360 File Offset: 0x00054760
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
