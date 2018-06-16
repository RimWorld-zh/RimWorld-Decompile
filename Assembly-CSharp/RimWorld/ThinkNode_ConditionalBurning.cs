using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001CC RID: 460
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x0005634C File Offset: 0x0005474C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
